<template>
  <div class="student-register-page">
    <div class="header">
      <h1>Opret elever</h1>
      <p class="subtitle">Opret elever manuelt eller upload CSV i bulk.</p>
    </div>

    <section class="grid">
      <div class="card">
        <h2>Manuel registration</h2>

        <label>
          <span>Fornavn</span>
          <input v-model="manual.firstName" type="text" />
        </label>

        <label>
          <span>Efternavn</span>
          <input v-model="manual.lastName" type="text" />
        </label>

        <label>
          <span>Email</span>
          <input v-model="manual.email" type="email" />
        </label>

        <label>
          <span>Kodeord</span>
          <input v-model="manual.password" type="password" />
        </label>

        <button @click="createManual">Opret elev</button>
      </div>

      <div class="card">
        <h2>CSV upload</h2>
        <p class="helper">Format: Fornavn,Efternavn,Email,Kodeord</p>
        <input type="file" accept=".csv" @change="onCsvSelected" />
        <button @click="createFromCsv" :disabled="!csvFile">Upload CSV</button>

        <p v-if="csvResult" class="csv-result">{{ csvResult }}</p>
      </div>
    </section>

    <section class="card roster">
      <h2>Oprettede elever ({{ students.length }})</h2>
      <div v-if="students.length === 0" class="helper">Ingen elever er oprettet endnu.</div>
      <ul v-else>
        <li v-for="student in students" :key="student.id" class="student-row">
          <span>{{ student.firstName }} {{ student.lastName }} · {{ student.email }}</span>
        </li>
      </ul>
    </section>

    <p v-if="status" :class="['status', statusType]">{{ status }}</p>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue';
import { RegisterStudent } from '../Services/api';

const manual = reactive({
  firstName: '',
  lastName: '',
  email: '',
  password: ''
});

const csvFile = ref(null);
const csvResult = ref('');
const students = ref([]); // shows newly created students, gone if refresh
const status = ref('');
const statusType = ref('ok');

async function createManual() {
  status.value = '';

  if (!manual.firstName || !manual.lastName || !manual.email || !manual.password) {
    status.value = 'All fields are required.';
    statusType.value = 'error';
    return;
  }

  try {
    const created = await RegisterStudent(
      manual.firstName,
      manual.lastName,
      manual.email,
      manual.password
    );

    students.value.unshift(created);

    manual.firstName = '';
    manual.lastName = '';
    manual.email = '';
    manual.password = '';

    status.value = 'Student created successfully.';
    statusType.value = 'ok';
  } catch (error) {
    status.value = 'Could not create student. Email may already exist.';
    statusType.value = 'error';
  }
}
// Not tested AI implementation 
function onCsvSelected(event) {
  const file = event.target.files?.[0];
  csvFile.value = file || null;
}
// Not tested AI implementation 
async function createFromCsv() {
  status.value = '';
  csvResult.value = '';

  if (!csvFile.value) {
    status.value = 'Select a CSV file first.';
    statusType.value = 'error';
    return;
  }

  const csvText = await csvFile.value.text();
  const rows = csvText
    .split(/\r?\n/)
    .map((line) => line.trim())
    .filter(Boolean);

  const created = [];
  let skipped = 0;

  for (const row of rows) {
    const [firstName, lastName, email, password] = row.split(',').map((x) => x?.trim());
    if (!firstName || !lastName || !email || !password) {
      skipped += 1;
      continue;
    }

    try {
      const student = await RegisterStudent(firstName, lastName, email, password);
      created.push(student);
    } catch {
      skipped += 1;
    }
  }

  if (created.length > 0) {
    students.value = [...created, ...students.value];
  }

  csvResult.value = `${created.length} created, ${skipped} skipped.`;
  status.value = 'CSV import completed.';
  statusType.value = 'ok';
}
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

.student-row {
  margin-bottom: 0.4rem;
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