<template>
  <div class="student-register-page">
    <div class="header">
      <p class="badge">Student Onboarding</p>
      <h1>Register Students</h1>
      <p class="subtitle">Create accounts manually or upload CSV in bulk.</p>
    </div>

    <section class="grid">
      <div class="card">
        <h2>Manual registration</h2>

        <label>
          <span>First name</span>
          <input v-model="manual.firstName" type="text" />
        </label>

        <label>
          <span>Last name</span>
          <input v-model="manual.lastName" type="text" />
        </label>

        <label>
          <span>Email</span>
          <input v-model="manual.email" type="email" />
        </label>

        <label>
          <span>Password</span>
          <input v-model="manual.password" type="password" />
        </label>

        <button @click="createManual">Create student</button>
      </div>

      <div class="card">
        <h2>CSV upload</h2>
        <p class="helper">Format: firstName,lastName,email,password</p>
        <input type="file" accept=".csv" @change="onCsvSelected" />
        <button @click="createFromCsv" :disabled="!csvFile">Upload CSV</button>

        <p v-if="csvResult" class="csv-result">{{ csvResult }}</p>
      </div>
    </section>

    <section class="card roster">
      <h2>Created students ({{ students.length }})</h2>
      <div v-if="students.length === 0" class="helper">No students created yet.</div>
      <ul v-else>
        <li v-for="student in students" :key="student.id">{{ student.firstName }} {{ student.lastName }} · {{ student.email }}</li>
      </ul>
    </section>

    <p v-if="status" :class="['status', statusType]">{{ status }}</p>
  </div>
</template>

<script setup>
import { onMounted, reactive, ref } from 'vue';
import {
  GetStudents,
  RegisterStudentLocal,
  RegisterStudentsFromCsvLocal
} from '../Services/api';

const manual = reactive({
  firstName: '',
  lastName: '',
  email: '',
  password: ''
});

const csvFile = ref(null);
const csvResult = ref('');
const students = ref([]);
const status = ref('');
const statusType = ref('ok');

function refreshStudents() {
  students.value = GetStudents();
}

function getTeacherEmail() {
  const user = JSON.parse(localStorage.getItem('user') || '{}');
  return user.email || 'unknown@teacher.local';
}

function createManual() {
  status.value = '';

  if (!manual.firstName || !manual.lastName || !manual.email || !manual.password) {
    status.value = 'All fields are required.';
    statusType.value = 'error';
    return;
  }

  try {
    RegisterStudentLocal({
      ...manual,
      teacherEmail: getTeacherEmail()
    });

    manual.firstName = '';
    manual.lastName = '';
    manual.email = '';
    manual.password = '';

    refreshStudents();
    status.value = 'Student created successfully.';
    statusType.value = 'ok';
  } catch (error) {
    status.value = 'Could not create student. Email may already exist.';
    statusType.value = 'error';
  }
}

function onCsvSelected(event) {
  const file = event.target.files?.[0];
  csvFile.value = file || null;
}

async function createFromCsv() {
  status.value = '';
  csvResult.value = '';

  if (!csvFile.value) {
    status.value = 'Select a CSV file first.';
    statusType.value = 'error';
    return;
  }

  const csvText = await csvFile.value.text();
  const { created, skipped } = RegisterStudentsFromCsvLocal(csvText, getTeacherEmail());

  refreshStudents();
  csvResult.value = `${created.length} created, ${skipped} skipped.`;
  status.value = 'CSV import completed.';
  statusType.value = 'ok';
}

onMounted(refreshStudents);
</script>

<style scoped>
.student-register-page {
  min-height: calc(100vh - 52px);
  overflow-y: auto;
  padding: 1.25rem;
  background: linear-gradient(155deg, #f4f8fc 0%, #ecf8ef 55%, #fffaef 100%);
}

.header {
  max-width: 1050px;
  margin: 0 auto 1rem;
}

.badge {
  display: inline-block;
  background: #e4f4ec;
  color: #145b45;
  border: 1px solid #c2dfd1;
  border-radius: 999px;
  padding: 0.25rem 0.65rem;
  font-size: 0.8rem;
  text-transform: uppercase;
}

.subtitle,
.helper {
  color: #5e6c77;
}

.grid {
  max-width: 1050px;
  margin: 0 auto;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.card {
  background: #fff;
  border: 1px solid #d8e5dc;
  border-radius: 14px;
  box-shadow: 0 8px 20px rgba(15, 23, 42, 0.08);
  padding: 1rem;
  display: grid;
  gap: 0.7rem;
}

label {
  display: grid;
  gap: 0.35rem;
}

input,
button {
  border-radius: 10px;
  border: 1px solid #c7d8ce;
  padding: 0.65rem 0.75rem;
}

button {
  border: none;
  background: #1f8a70;
  color: #fff;
  font-weight: 600;
  cursor: pointer;
}

button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.roster {
  max-width: 1050px;
  margin: 1rem auto 0;
}

.roster ul {
  margin: 0;
  padding-left: 1rem;
}

.status {
  max-width: 1050px;
  margin: 1rem auto 0;
  border-radius: 8px;
  padding: 0.65rem;
}

.status.ok {
  background: #e7f8ef;
  color: #14532d;
}

.status.error {
  background: #fee8e8;
  color: #7f1d1d;
}

.csv-result {
  color: #14532d;
  font-weight: 600;
}

@media (max-width: 900px) {
  .grid {
    grid-template-columns: 1fr;
  }
}
</style>
