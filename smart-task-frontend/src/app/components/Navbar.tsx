"use client";

import Link from "next/link";
import { useState } from "react";
// optional: import useAuthStore if you have it
// import { useAuthStore } from "@/stores/authStore";

export default function Navbar() {
  const [open, setOpen] = useState(false);
  // const token = useAuthStore((s) => s.token); // falls du authStore benutzt

  return (
    <nav className="navbar bg-base-100 shadow-md rounded-lg mb-4">
      <div className="flex-1">
        <Link href="/" className="btn btn-ghost normal-case text-xl">
          smartTask
        </Link>
      </div>

      <div className="flex-none">
        <div className="hidden sm:flex gap-2 items-center">
          <Link href="/dashboard" className="btn btn-ghost">
            Dashboard
          </Link>
          <Link href="/tasks" className="btn btn-ghost">
            Tasks
          </Link>
        </div>

        {/* simple auth buttons (replace with authStore usage if available) */}
        <div className="ml-3">
          <Link href="/login" className="btn btn-primary">
            Login
          </Link>
        </div>

        {/* mobile menu toggle */}
        <button
          onClick={() => setOpen((s) => !s)}
          className="btn btn-square btn-ghost sm:hidden ml-2"
          aria-label="menu"
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16" />
          </svg>
        </button>
      </div>

      {open && (
        <div className="w-full sm:hidden mt-2">
          <div className="flex flex-col gap-2">
            <Link href="/dashboard" className="btn btn-ghost w-full">Dashboard</Link>
            <Link href="/tasks" className="btn btn-ghost w-full">Tasks</Link>
            <Link href="/login" className="btn btn-primary w-full">Login</Link>
          </div>
        </div>
      )}
    </nav>
  );
}
