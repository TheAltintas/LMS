import { createRouter, createWebHistory } from 'vue-router';

import Login from '../Pages/Login.vue';
import Register from '../Pages/Register.vue';
import TeacherDashboard from '../Pages/TeacherDashboard.vue';
import FrontPage from '../Pages/FrontPage.vue';
import TaskCreation from '../Pages/TaskCreation.vue';
import TasksetCreation from '../Pages/TasksetCreation.vue';
import RegisterStudent from '../Pages/RegisterStudent.vue';
import StudentLogin from '../Pages/StudentLogin.vue';
import StudentDashboard from '../Pages/StudentDashboard.vue';
const routes = [
  { path: '/', name: 'FrontPage', component: FrontPage, meta: { guestOnly: true } },
  { path: '/login', name: 'Login', component: Login, meta: { guestOnly: true } },
  { path: '/student-login', name: 'StudentLogin', component: StudentLogin, meta: { guestOnly: true } },
  { path: '/register', name: 'Register', component: Register, meta: { guestOnly: true } },
  { path: '/teacher-dashboard', name: 'TeacherDashboard', component: TeacherDashboard, meta: { requiresAuth: true, requiresTeacher: true } },
  { path: '/student-dashboard', name: 'StudentDashboard', component: StudentDashboard, meta: { requiresAuth: true, requiresStudent: true } },
  { path: '/create-task', name: 'TaskCreation', component: TaskCreation, meta: { requiresAuth: true, requiresTeacher: true } },
  { path: '/create-taskset', name: 'TasksetCreation', component: TasksetCreation, meta: { requiresAuth: true, requiresTeacher: true } },
  { path: '/register-student', name: 'RegisterStudent', component: RegisterStudent, meta: { requiresAuth: true, requiresTeacher: true } }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const user = JSON.parse(localStorage.getItem('user') || 'null');
  const loggedIn = !!user;

  // Guest-only pages
  if (to.meta.guestOnly && loggedIn) {
    // Redirect teachers to dashboard - in future redirect students to /student-dashboard
    if (user?.role?.toLowerCase() === 'teacher') {
      return next('/teacher-dashboard');
    }
    if (user?.role?.toLowerCase() === 'student') {
      return next('/student-dashboard');
    }
    // Redirect other logged-in users to a default page
    return next('/'); 
  }

  // Require login
  if (to.meta.requiresAuth && !loggedIn) {
    return next('/login');
  }

  // Require teacher role
  if (to.meta.requiresTeacher && user?.role?.toLowerCase() !== 'teacher') {
    return next('/');
  }

  if (to.meta.requiresStudent && user?.role?.toLowerCase() !== 'student') {
    return next('/');
  }

  next();
});


export default router;