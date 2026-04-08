<template>
  <div class="taskset-page">
    <div class="page-head">
      <p class="badge">Taskset Builder</p>
      <h1>Create Taskset</h1>
      <p class="subtitle">Search with filters, pick multiple tasks, and generate a printable PDF.</p>
    </div>

    <section class="panel filters">
      <h2>Search Tasks</h2>
      <div class="filter-grid">
        <label>
          <span>Subject</span>
          <input v-model="filters.subject" type="text" placeholder="Mathematics" />
        </label>

        <label>
          <span>Niveau</span>
          <select v-model="filters.classLevel">
            <option value="">All</option>
            <option value="A">A</option>
            <option value="B">B</option>
            <option value="C">C</option>
          </select>
        </label>

        <label>
          <span>Delprøve</span>
          <select v-model="filters.type">
            <option value="">All</option>
            <option value="Delprøve 1">Delprøve 1</option>
            <option value="Delprøve 2">Delprøve 2</option>
            <option value="Mundtlig">Mundtlig</option>
          </select>
        </label>

        <label>
          <span>Point min</span>
          <input v-model.number="filters.minPoints" type="number" min="0" step="0.5" />
        </label>

        <label>
          <span>Point max</span>
          <input v-model.number="filters.maxPoints" type="number" min="0" step="0.5" />
        </label>
      </div>

      <div class="actions">
        <button @click="loadTasks" :disabled="loading">{{ loading ? 'Loading...' : 'Refresh tasks' }}</button>
        <button class="ghost" @click="clearFilters">Clear filters</button>
      </div>
    </section>

    <section class="layout">
      <div class="panel">
        <h2>Task List ({{ filteredTasks.length }})</h2>

        <div v-if="filteredTasks.length === 0" class="empty">No tasks match current filters.</div>

        <div v-else class="task-list">
          <label v-for="task in filteredTasks" :key="task.id" class="task-item">
            <input v-model="selectedTaskIds" :value="task.id" type="checkbox" />
            <div>
              <p class="title">#{{ task.id }} {{ task.subject }}</p>
              <p class="meta">Niveau {{ task.classLevel }} · {{ task.type }} · {{ task.points }} points</p>
            </div>
          </label>
        </div>
      </div>

      <div class="panel">
        <h2>Taskset</h2>

        <label>
          <span>Taskset Name</span>
          <input v-model="tasksetName" type="text" placeholder="Term Exam - Delprøve 1" />
        </label>

        <div class="selection-box">
          <p><strong>{{ selectedTasks.length }}</strong> selected</p>
          <ul>
            <li v-for="task in selectedTasks" :key="task.id">{{ task.subject }} · {{ task.classLevel }} · {{ task.points }}</li>
          </ul>
        </div>

        <div class="actions">
          <button @click="saveTaskset">Save taskset object</button>
          <button class="ghost" @click="generatePdf" :disabled="selectedTasks.length === 0">Generate PDF</button>
        </div>

        <p v-if="status" :class="['status', statusType]">{{ status }}</p>
      </div>
    </section>
  </div>
</template>

<script setup>
import { computed, onMounted, reactive, ref } from 'vue';
import { SaveTaskset, SearchTasks } from '../Services/api';

const loading = ref(false);
const status = ref('');
const statusType = ref('ok');
const allTasks = ref([]);
const selectedTaskIds = ref([]);
const tasksetName = ref('');

const filters = reactive({
  subject: '',
  classLevel: '',
  type: '',
  minPoints: '',
  maxPoints: ''
});

function normalize(value) {
  return (value || '').toString().toLowerCase();
}

const filteredTasks = computed(() => {
  return allTasks.value.filter((task) => {
    const subjectMatch = !filters.subject || normalize(task.subject).includes(normalize(filters.subject));
    const levelMatch = !filters.classLevel || normalize(task.classLevel) === normalize(filters.classLevel);
    const typeMatch = !filters.type || normalize(task.type) === normalize(filters.type);
    const minMatch = filters.minPoints === '' || Number(task.points) >= Number(filters.minPoints);
    const maxMatch = filters.maxPoints === '' || Number(task.points) <= Number(filters.maxPoints);

    return subjectMatch && levelMatch && typeMatch && minMatch && maxMatch;
  });
});

const selectedTasks = computed(() =>
  allTasks.value.filter((task) => selectedTaskIds.value.includes(task.id))
);

function clearFilters() {
  filters.subject = '';
  filters.classLevel = '';
  filters.type = '';
  filters.minPoints = '';
  filters.maxPoints = '';
}

async function loadTasks() {
  loading.value = true;
  status.value = '';

  try {
    allTasks.value = await SearchTasks();
  } catch {
    allTasks.value = [];
    status.value = 'Could not load tasks.';
    statusType.value = 'error';
  } finally {
    loading.value = false;
  }
}

function saveTaskset() {
  status.value = '';

  if (!tasksetName.value.trim()) {
    status.value = 'Taskset name is required.';
    statusType.value = 'error';
    return;
  }

  if (selectedTasks.value.length === 0) {
    status.value = 'Select at least one task.';
    statusType.value = 'error';
    return;
  }

  const user = JSON.parse(localStorage.getItem('user') || '{}');

  SaveTaskset({
    name: tasksetName.value.trim(),
    teacherEmail: user.email || 'unknown',
    taskIds: selectedTasks.value.map((task) => task.id),
    tasks: selectedTasks.value
  });

  status.value = 'Taskset object saved in browser storage.';
  statusType.value = 'ok';
}

function generatePdf() {
  const html = `
    <html>
      <head>
        <title>${tasksetName.value || 'Taskset'}</title>
        <style>
          body { font-family: 'Segoe UI', Arial, sans-serif; padding: 24px; }
          h1 { margin: 0 0 8px; }
          p { margin: 0 0 16px; color: #555; }
          table { width: 100%; border-collapse: collapse; }
          th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
          th { background: #f2f5f8; }
        </style>
      </head>
      <body>
        <h1>${tasksetName.value || 'Taskset'}</h1>
        <p>Generated: ${new Date().toLocaleString()}</p>
        <table>
          <thead>
            <tr><th>ID</th><th>Subject</th><th>Niveau</th><th>Delprøve</th><th>Points</th></tr>
          </thead>
          <tbody>
            ${selectedTasks.value.map((task) =>
              `<tr><td>${task.id}</td><td>${task.subject}</td><td>${task.classLevel}</td><td>${task.type}</td><td>${task.points}</td></tr>`
            ).join('')}
          </tbody>
        </table>
      </body>
    </html>
  `;

  const popup = window.open('', '_blank');
  if (!popup) {
    status.value = 'Popup blocked. Allow popups to generate PDF.';
    statusType.value = 'error';
    return;
  }

  popup.document.write(html);
  popup.document.close();
  popup.focus();
  popup.print();
}

onMounted(loadTasks);
</script>

<style scoped>
.taskset-page {
  min-height: calc(100vh - 52px);
  overflow-y: auto;
  padding: 1.25rem;
  background: linear-gradient(165deg, #f3f7fb 0%, #ebf8f0 45%, #fff9ef 100%);
}

.page-head {
  max-width: 1200px;
  margin: 0 auto 1rem;
}

.badge {
  display: inline-block;
  background: #dff4eb;
  color: #12654d;
  border: 1px solid #b6dfcf;
  border-radius: 999px;
  padding: 0.25rem 0.7rem;
  font-size: 0.8rem;
  text-transform: uppercase;
}

h1 {
  margin: 0.5rem 0 0.35rem;
}

.subtitle {
  color: #556;
}

.panel {
  background: #fff;
  border: 1px solid #dae7df;
  border-radius: 14px;
  box-shadow: 0 8px 20px rgba(15, 23, 42, 0.08);
  padding: 1rem;
}

.filters {
  max-width: 1200px;
  margin: 0 auto 1rem;
}

.filter-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
  gap: 0.75rem;
}

label {
  display: grid;
  gap: 0.35rem;
}

input,
select,
button {
  border-radius: 10px;
  border: 1px solid #c7d8ce;
  padding: 0.65rem 0.75rem;
}

.actions {
  margin-top: 0.8rem;
  display: flex;
  gap: 0.6rem;
}

button {
  border: none;
  background: #1f8a70;
  color: #fff;
  cursor: pointer;
}

button.ghost {
  background: #eff6f2;
  border: 1px solid #bed3c7;
  color: #20493e;
}

.layout {
  max-width: 1200px;
  margin: 0 auto;
  display: grid;
  grid-template-columns: 1.1fr 0.9fr;
  gap: 1rem;
}

.task-list {
  margin-top: 0.5rem;
  display: grid;
  gap: 0.5rem;
}

.task-item {
  display: grid;
  grid-template-columns: auto 1fr;
  gap: 0.55rem;
  border: 1px solid #e2ebe6;
  border-radius: 10px;
  padding: 0.65rem;
}

.title {
  font-weight: 600;
}

.meta {
  color: #5e6b76;
}

.selection-box {
  margin-top: 0.7rem;
  border: 1px dashed #bcd2c6;
  border-radius: 10px;
  padding: 0.75rem;
  max-height: 220px;
  overflow-y: auto;
}

.selection-box ul {
  padding-left: 1rem;
  margin: 0.4rem 0 0;
}

.status {
  margin-top: 0.75rem;
  border-radius: 8px;
  padding: 0.6rem;
}

.status.ok {
  background: #e7f8ef;
  color: #14532d;
}

.status.error {
  background: #fee8e8;
  color: #7f1d1d;
}

.empty {
  margin-top: 0.7rem;
  color: #6c7883;
}

@media (max-width: 900px) {
  .layout {
    grid-template-columns: 1fr;
  }
}
</style>
