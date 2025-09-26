// src/lib/api.ts
export const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5284/api";


/**
 * Generic POST helper with normalized server error extraction.
 * Usage: await apiPost("/auth/register", { username, password, role })
 */
export async function apiPost<T>(path: string, body: unknown): Promise<T> {
  const res = await fetch(`${API_URL}${path}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    // try read body as text / json to provide useful message
    let text = await res.text().catch(() => res.statusText ?? `HTTP ${res.status}`);
    try {
      const json = JSON.parse(text);
      text = json?.message ?? json?.error ?? text;
    } catch {
      // not JSON - keep text
    }
    throw new Error(text || `HTTP ${res.status}`);
  }

  // assume JSON response
  return (await res.json()) as T;
}

/**
 * Generic fetch wrapper (GET, PUT, etc.)
 * Automatically attaches Authorization header if token present in localStorage.
 */
export async function apiFetch<T>(url: string, options: RequestInit = {}): Promise<T> {
  const token = typeof window !== "undefined" ? localStorage.getItem("token") : null;

  const res = await fetch(`${API_URL}${url}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options.headers,
    },
  });

  if (!res.ok) {
    const text = await res.text().catch(() => res.statusText ?? `HTTP ${res.status}`);
    throw new Error(text || `HTTP ${res.status}`);
  }
  return (await res.json()) as T;
}
