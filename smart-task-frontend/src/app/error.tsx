"use client";

import { useEffect } from "react";

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    console.error("App Error:", error);
  }, [error]);

  return (
    <div className="hero min-h-screen bg-base-200">
      <div className="hero-content text-center">
        <div className="max-w-md">
          <h1 className="text-5xl font-bold text-error">Oops!</h1>
          <p className="py-6 text-lg">
            Etwas ist schiefgelaufen. Bitte versuche es später erneut.
          </p>
          <button onClick={() => reset()} className="btn btn-primary">
            Neu laden
          </button>
        </div>
      </div>
    </div>
  );
}
