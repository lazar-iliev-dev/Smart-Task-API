import Link from "next/link";

export default function NotFound() {
  return (
    <div className="hero min-h-screen bg-base-200">
      <div className="hero-content text-center">
        <div className="max-w-md">
          <h1 className="text-9xl font-bold text-error">404</h1>
          <p className="py-6 text-lg">
            Ups! Die Seite, die du suchst, existiert nicht oder wurde verschoben.
          </p>
          <Link href="/" className="btn btn-primary">
            Zurück zur Startseite
          </Link>
        </div>
      </div>
    </div>
  );
}
