<template>
  <div class="student-login-page">
    <form class="card" @submit.prevent="handleLogin" novalidate>
      <p class="badge">Student</p>
      <h1>Student Login</h1>

      <label>
        <span>Email</span>
        <input v-model="email" type="email" required />
      </label>

      <label>
        <span>Password</span>
        <input v-model="password" type="password" required />
      </label>

      <button type="submit" :disabled="loading">{{ loading ? 'Signing in...' : 'Sign in' }}</button>

      <p v-if="status" :class="['status', statusType]">{{ status }}</p>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { LoginStudentLocal } from '../Services/api';

const router = useRouter();
const email = ref('');
const password = ref('');
const loading = ref(false);
const status = ref('');
const statusType = ref('ok');

async function handleLogin() {
  status.value = '';
  loading.value = true;

  try {
    const student = LoginStudentLocal(email.value, password.value);

    if (!student) {
      status.value = 'Invalid email or password.';
      statusType.value = 'error';
      return;
    }

    localStorage.setItem('user', JSON.stringify({
      role: 'Student',
      email: student.email,
      studentId: student.id
    }));

    status.value = 'Login successful. Redirecting...';
    statusType.value = 'ok';

    setTimeout(() => {
      router.push('/student-dashboard');
    }, 500);
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.student-login-page {
  min-height: calc(100vh - 52px);
  display: grid;
  place-items: center;
  padding: 1rem;
  background: linear-gradient(150deg, #f1f7ff 0%, #edf9f2 50%, #fffaf1 100%);
}

.card {
  width: min(460px, 100%);
  background: #fff;
  border: 1px solid #d6e4dc;
  border-radius: 14px;
  padding: 1.25rem;
  box-shadow: 0 10px 24px rgba(15, 23, 42, 0.1);
  display: grid;
  gap: 0.75rem;
}

.badge {
  display: inline-block;
  width: fit-content;
  padding: 0.2rem 0.6rem;
  font-size: 0.8rem;
  text-transform: uppercase;
  border-radius: 999px;
  background: #e5f5ed;
  color: #155d47;
}

label {
  display: grid;
  gap: 0.35rem;
}

input,
button {
  border-radius: 10px;
  border: 1px solid #c6d7cd;
  padding: 0.65rem 0.75rem;
}

button {
  border: none;
  background: #1f8a70;
  color: #fff;
  font-weight: 700;
  cursor: pointer;
}

.status {
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
</style>
