const API_BASE_URL = 'http://localhost:5294/api';

async function request(path, options = {}) {
  const response = await fetch(`${API_BASE_URL}${path}`, options);

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || 'Request failed');
  }

  return await response.json();
}

function readLocalJson(key, fallback) {
  try {
    const value = localStorage.getItem(key);
    return value ? JSON.parse(value) : fallback;
  } catch {
    return fallback;
  }
}

function writeLocalJson(key, value) {
  localStorage.setItem(key, JSON.stringify(value));
}

function normalizeTask(task) {
  return {
    id: task.id,
    subject: task.subject,
    classLevel: task.classLevel,
    type: task.type,
    points: Number(task.points),
    pictureUrl: task.pictureUrl ?? null,
    videoUrl: task.videoUrl ?? null,
    createdDate: task.createdDate ?? new Date().toISOString()
  };
}

function getLocalTasks() {
  return readLocalJson('tasks', []).map(normalizeTask);
}

function saveLocalTask(taskData) {
  const tasks = getLocalTasks();
  const nextTask = normalizeTask({
    ...taskData,
    id: Date.now() + Math.floor(Math.random() * 1000),
    createdDate: new Date().toISOString()
  });

  tasks.push(nextTask);
  writeLocalJson('tasks', tasks);
  return nextTask;
}

function mergeTasks(remoteTasks, localTasks) {
  const merged = [];
  const seen = new Set();

  const taskKey = (task) => {
    if (task.id !== undefined && task.id !== null) {
      return `id-${task.id}`;
    }

    return `${task.subject}|${task.classLevel}|${task.type}|${task.points}`;
  };

  for (const task of [...remoteTasks, ...localTasks]) {
    const normalized = normalizeTask(task);
    const key = taskKey(normalized);

    if (!seen.has(key)) {
      seen.add(key);
      merged.push(normalized);
    }
  }

  return merged;
}

export async function RegisterTeacher(firstName, lastName, email, password) {
  return request('/teacher', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ firstName, lastName, email, password })
  });
}

export async function LoginTeacher(email, password) {
  return request('/teacher/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
}

export async function CreateTask(taskData) {
  try {
    return await request('/assignment', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(taskData)
    });
  } catch {
    return saveLocalTask(taskData);
  }
}

export async function SearchTasks() {
  const localTasks = getLocalTasks();

  try {
    const remoteTasks = await request('/assignment', { method: 'GET' });
    return mergeTasks(remoteTasks, localTasks);
  } catch {
    return localTasks;
  }
}

export function SaveTaskset(taskset) {
  const tasksets = readLocalJson('tasksets', []);
  const nextTaskset = {
    ...taskset,
    id: Date.now(),
    createdAt: new Date().toISOString()
  };

  tasksets.push(nextTaskset);
  writeLocalJson('tasksets', tasksets);

  return nextTaskset;
}

export function GetStudents() {
  return readLocalJson('students', []);
}

export function RegisterStudentLocal(student) {
  const students = readLocalJson('students', []);
  const duplicate = students.some((s) => s.email.toLowerCase() === student.email.toLowerCase());

  if (duplicate) {
    throw new Error('Student email already exists');
  }

  const nextStudent = {
    ...student,
    id: Date.now() + Math.floor(Math.random() * 1000),
    createdAt: new Date().toISOString()
  };

  students.push(nextStudent);
  writeLocalJson('students', students);

  return nextStudent;
}

export function RegisterStudentsFromCsvLocal(csvText, teacherEmail) {
  const lines = csvText.split(/\r?\n/).filter((line) => line.trim().length > 0);
  if (lines.length === 0) {
    return { created: [], skipped: 0 };
  }

  let startIndex = 0;
  const firstLine = lines[0].toLowerCase();
  const hasHeader = firstLine.includes('firstname') || firstLine.includes('email');
  if (hasHeader) {
    startIndex = 1;
  }

  const created = [];
  let skipped = 0;

  for (let i = startIndex; i < lines.length; i += 1) {
    const row = lines[i].split(',').map((item) => item.trim());

    if (row.length < 4) {
      skipped += 1;
      continue;
    }

    const [firstName, lastName, email, password] = row;

    if (!firstName || !lastName || !email || !password) {
      skipped += 1;
      continue;
    }

    try {
      const student = RegisterStudentLocal({ firstName, lastName, email, password, teacherEmail });
      created.push(student);
    } catch {
      skipped += 1;
    }
  }

  return { created, skipped };
}

export function LoginStudentLocal(email, password) {
  const students = readLocalJson('students', []);

  const student = students.find((s) =>
    s.email.toLowerCase() === email.toLowerCase() && s.password === password
  );

  return student || null;
}
