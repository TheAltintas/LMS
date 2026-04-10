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

function parseJwtPayload(token) {
  try {
    const base64 = token.split('.')[1];
    if (!base64) return null;

    const normalized = base64.replace(/-/g, '+').replace(/_/g, '/');
    const padded = normalized.padEnd(normalized.length + ((4 - normalized.length % 4) % 4), '=');
    return JSON.parse(atob(padded));
  } catch {
    return null;
  }
}

function getTeacherIdFromToken() {
  const token = getAccessToken();
  if (!token) return null;

  const payload = parseJwtPayload(token);
  const teacherIdRaw = payload?.nameid ?? payload?.sub;
  const teacherId = Number(teacherIdRaw);

  return Number.isInteger(teacherId) ? teacherId : null;
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
    const error = new Error(text || `Request failed (${response.status})`);
    error.status = response.status;
    throw error;
  }

  if (response.status === 204) {
    return null;
  }

  const contentType = response.headers.get('content-type') || '';
  if (contentType.toLowerCase().includes('application/json')) {
    return await response.json();
  }

  return await response.text();
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

export async function CreateAssignment(data) {
  return await request('/assignment', {
    method: 'POST',
    body: JSON.stringify(data)
  }, true);
}

export async function GetTeacherAssignments() {
  return await request('/assignment/teacher', {
    method: 'GET'
  }, true);
}

export async function CreateAssignmentSet(name) {
  const teacherId = getTeacherIdFromToken();
  const payload = teacherId ? { name, teacherId } : { name };

  return await request('/assignmentset', {
    method: 'POST',
    body: JSON.stringify(payload)
  }, true);
}

export async function GetTeacherAssignmentSets() {
  try {
    return await request('/assignmentset', {
      method: 'GET'
    }, true);
  } catch (error) {
    if ((error?.message || '').toLowerCase().includes('no assignment sets found')) {
      return [];
    }

    if (error?.status === 404) {
      const teacherId = getTeacherIdFromToken();
      if (!teacherId) {
        throw new Error('Could not resolve teacher id from JWT token.');
      }

      return await request(`/assignmentset/teacher/${teacherId}`, {
        method: 'GET'
      }, true);
    }

    throw error;
  }
}

export async function AddAssignmentToAssignmentSet(assignmentSetId, assignmentId) {
  return await request(`/assignmentset/${assignmentSetId}/add-assignment/${assignmentId}`, {
    method: 'POST'
  }, true);
}