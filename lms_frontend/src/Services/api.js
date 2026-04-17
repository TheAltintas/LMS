export async function RegisterTeacher(firstName, lastName,email, password) {
    const response = await fetch('http://localhost:8080/api/teacher', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ firstName, lastName, email, password })
    });

    if (!response.ok) {
        const text = await response.text();
        throw new Error(text);
    }

    return await response.json();
}

export async function LoginTeacher(email, password) {
    const response = await fetch('http://localhost:8080/api/teacher/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email, password })
    });

    if (!response.ok) {
        const text = await response.text();
        throw new Error(text);
    }

    return await response.json();
}

export async function CreateTask(data) {
  console.log("Sending payload:", data);

  const response = await fetch('http://localhost:8080/api/assignment', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text);
  }

  return await response.json();
}
