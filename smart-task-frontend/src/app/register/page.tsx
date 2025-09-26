"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { apiPost } from "../lib/api";
import { getErrorMessage } from "../lib/errors";

type Role = "USER" | "ADMIN";

export default function RegisterPage() {
  const router = useRouter();
  // dann beim Submit: username = emailValue

  const [usernameValue, setUsernameValue] = useState(""); // or email input mapped to username
  const [password, setPassword] = useState("");
  const [confirm, setConfirm] = useState("");
  const [role, setRole] = useState<Role>("USER");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showEmailInstead, setShowEmailInstead] = useState(false); // toggle if you want Email label

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);

    if (password.length < 6) {
      setError("Das Passwort muss mindestens 6 Zeichen lang sein.");
      return;
    }
    if (password !== confirm) {
      setError("Passwörter stimmen nicht überein.");
      return;
    }

    setLoading(true);
    try {
      const payload = {
        username: usernameValue.trim(), // wenn du E-Mail benutzt, wird sie hier als username gemappt
        password,
        role: role ?? "USER",
      };

      // ruft (z.B.) http://localhost:5000/api/auth/register auf
      await apiPost("/auth/register", payload);

      // bei Erfolg weiterleiten
      router.push("/login");
    } catch (err: unknown) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="hero min-h-screen bg-base-200">
      <div className="card w-full max-w-lg bg-base-100 shadow-xl">
        <div className="card-body">
          <h2 className="card-title">Registrieren</h2>

          <div className="form-control">
            <label className="label cursor-pointer">
              <span className="label-text">E-Mail statt Username anzeigen?</span>
              <input
                type="checkbox"
                className="toggle toggle-sm"
                checked={showEmailInstead}
                onChange={(e) => setShowEmailInstead(e.target.checked)}
              />
            </label>
          </div>

          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="form-control">
              <label className="label">
                <span className="label-text">
                  {showEmailInstead ? "E-Mail" : "Benutzername"}
                </span>
              </label>
              <input
                type={showEmailInstead ? "email" : "text"}
                placeholder={showEmailInstead ? "me@example.com" : "username"}
                className="input input-bordered w-full"
                value={usernameValue}
                onChange={(e) => setUsernameValue(e.target.value)}
                required
              />
            </div>

            <div className="form-control">
              <label className="label"><span className="label-text">Passwort</span></label>
              <input
                type="password"
                placeholder="Passwort"
                className="input input-bordered w-full"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                minLength={6}
                required
              />
            </div>

            <div className="form-control">
              <label className="label"><span className="label-text">Passwort bestätigen</span></label>
              <input
                type="password"
                placeholder="Passwort bestätigen"
                className="input input-bordered w-full"
                value={confirm}
                onChange={(e) => setConfirm(e.target.value)}
                required
              />
            </div>

            <div className="form-control">
              <label className="label"><span className="label-text">Rolle</span></label>
              <select
                className="select select-bordered w-full"
                value={role}
                onChange={(e) => setRole(e.target.value as Role)}
                aria-label="Rolle auswählen"
              >
                <option value="USER">User</option>
                <option value="ADMIN">Admin</option>
              </select>
              <p className="text-xs text-neutral mt-1">
                Achtung: Admin-Rechte sollten in Produktion nicht frei wählbar sein.
              </p>
            </div>

            {error && <div className="text-sm text-error">{error}</div>}

            <div className="form-control mt-4">
              <button className="btn btn-primary" type="submit" disabled={loading}>
                {loading ? "Registriere..." : "Konto erstellen"}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
