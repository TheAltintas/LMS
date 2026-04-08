<template>
  <div class="student-dashboard">
    <div class="panel">
      <p class="badge">Student Area</p>
      <h1>Student Dashboard</h1>
      <p>This page is ready for upcoming assigned tasksets.</p>
    </div>
  </div>
</template>

<script setup>
</script>

<style scoped>
.student-dashboard {
  min-height: calc(100vh - 52px);
  display: grid;
  place-items: center;
  background: linear-gradient(150deg, #f1f8ff, #eefbf2);
  padding: 1rem;
}

.panel {
  width: min(900px, 100%);
  background: #fff;
  border: 1px solid #d7e4dd;
  border-radius: 14px;
  box-shadow: 0 10px 24px rgba(15, 23, 42, 0.08);
  padding: 1.5rem;
}

.badge {
  display: inline-block;
  border-radius: 999px;
  background: #e4f5ec;
  color: #145a44;
  text-transform: uppercase;
  font-size: 0.75rem;
  padding: 0.2rem 0.6rem;
}

h1 {
  margin: 0.45rem 0;
}
</style>
