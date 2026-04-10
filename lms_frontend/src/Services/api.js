const API_BASE_URL = 'http://localhost:5294/api';
const AUTH_STORAGE_KEY = 'auth';

export function getAuthSession() {
  try {
    const raw = localStorage.getItem(AUTH_STORAGE_KEY);
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
}

export function setAuthSession(authResponse) {
  localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(authResponse));
}

export function clearAuthSession() {
  localStorage.removeItem(AUTH_STORAGE_KEY);
}

export function getAccessToken() {
  return getAuthSession()?.token ?? null;
}

function buildHeaders(extraHeaders = {}, requiresAuth = false) {
  const headers = {
    'Content-Type': 'application/json',
    ...extraHeaders
  };

  if (requiresAuth) {
    const token = getAccessToken();
    if (!token) {
      throw new Error('Missing authentication token. Please log in again.');
    }
    headers.Authorization = `Bearer ${token}`;
  }

  return headers;
}

async function request(path, options = {}, requiresAuth = false) {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers: buildHeaders(options.headers, requiresAuth)
  });

  if (response.status === 401) {
    clearAuthSession();
    throw new Error('Unauthorized. Please log in again.');
  }

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || 'Request failed');
  }

  return await response.json();
}

export async function RegisterTeacher(firstName, lastName, email, password) {
  return await request('/teacher', {
    method: 'POST',
    body: JSON.stringify({ firstName, lastName, email, password })
  });
}

export async function LoginTeacher(email, password) {
  return await request('/teacher/login', {
    method: 'POST',
    body: JSON.stringify({ email, password })
  });
}

export async function LoginStudent(email, password) {
  return await request('/student/login', {
    method: 'POST',
    body: JSON.stringify({ email, password })
  });
}

export async function CreateTask(data) {
  return await request('/assignment', {
    method: 'POST',
    body: JSON.stringify(data)
  }, true);
}