// src/app/login/page.tsx
"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { apiPost } from "../lib/api";


export default function LoginPage() {
  const router = useRouter();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  function extractValidationMessage(msg: string): string {
    // Falls der Server ProblemDetails/ValidationErrors als JSON zurückgibt,
    // versuche den ersten Fehler sinnvoll zu extrahieren.
    try {
      const json = JSON.parse(msg);
      if (json?.errors && typeof json.errors === "object") {
        const keys = Object.keys(json.errors);
        if (keys.length > 0) {
          const first = json.errors[keys[0]];
          if (Array.isArray(first) && first.length > 0) return String(first[0]);
        }
      }
      if (json?.message) return String(json.message);
    } catch {
      // no-op
    }
    return msg;
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);

    if (!username.trim() || !password) {
      setError("Benutzername und Passwort sind erforderlich.");
      return;
    }

    const payload = { username: username.trim(), password };
    console.log("[Login] payload:", payload); // ---> prüfe das in DevTools Network/Console

    setLoading(true);
    try {
      const res = await apiPost<{ token: string }>("/auth/login", payload);
      // Erfolg: Token in localStorage (oder benutze deinen authStore)
      localStorage.setItem("token", res.token);
      router.push("/dashboard");
    } catch (err: unknown) {
      // err.message kann JSON-ProblemDetails oder plain text sein
      const raw = (err instanceof Error ? err.message : String(err)) ?? "Login fehlgeschlagen";
      const msg = extractValidationMessage(raw);
      setError(msg);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="flex items-center justify-center min-h-screen bg-base-200">
      <div className="card w-full max-w-md shadow-2xl bg-base-100">
        <div className="card-body">
          <h2 className="card-title text-center">Login</h2>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="form-control">
              <label className="label"><span className="label-text">Benutzername (oder E-Mail)</span></label>
              <input
                type="text"
                className="input input-bordered"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="username oder email"
                required
              />
            </div>

            <div className="form-control">
              <label className="label"><span className="label-text">Passwort</span></label>
              <input
                type="password"
                className="input input-bordered"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Dein Passwort"
                required
              />
            </div>

            {error && <div className="text-sm text-error">{error}</div>}

            <div className="form-control mt-4">
              <button className="btn btn-primary" type="submit" disabled={loading}>
                {loading ? "Anmelden..." : "Anmelden"}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
