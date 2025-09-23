"use client";

import Image from "next/image";
import Link from "next/link";
import { useState, useEffect } from "react";

export default function Home() {
  const [theme, setTheme] = useState<"light" | "dark">("light");
  const [user, setUser] = useState<{ name: string; avatar: string } | null>(
    null
  );

  // Apply theme when loading
  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
  }, [theme]);

  return (
    <div className="font-sans min-h-screen bg-base-200 text-base-content flex flex-col">
      {/* Navbar */}
      <header className="px-6 py-4">
        <nav className="navbar bg-base-100 shadow-sm rounded-lg">
          {/* Logo */}
          <div className="flex-1">
            <Link href="/" className="btn btn-ghost text-xl normal-case">
              smartTask
            </Link>
          </div>

          <div className="flex items-center gap-3">
            {/* Search Button statt Input */}
            <button className="btn btn-ghost flex gap-2">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-5 w-5"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M21 21l-4.35-4.35M11 19a8 8 0 100-16 8 8 0 000 16z"
                />
              </svg>
              <span>Search</span>
            </button>

            {/* Darkmode Toggle */}
            <label className="swap swap-rotate">
              <input
                type="checkbox"
                checked={theme === "dark"}
                onChange={() =>
                  setTheme((prev) => (prev === "light" ? "dark" : "light"))
                }
              />
              {/* Sun */}
              <svg
                className="swap-on h-6 w-6 fill-current"
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
              >
                <path d="M5.64 17.657l-1.414 1.415-2.121-2.122 1.414-1.414zM12 18a6 6 0 110-12 6 6 0 010 12zm7.071-7.071l1.415-1.415-2.122-2.121-1.414 1.414zM18 12a6 6 0 11-12 0 6 6 0 0112 0z" />
              </svg>
              {/* Moon */}
              <svg
                className="swap-off h-6 w-6 fill-current"
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
              >
                <path d="M21.64 13.65a9 9 0 11-11.31-11.31 9 9 0 0011.31 11.31z" />
              </svg>
            </label>

            {/* Avatar oder Login */}
            {user ? (
              <div className="dropdown dropdown-end">
                <button
                  tabIndex={0}
                  aria-haspopup="true"
                  className="btn btn-ghost btn-circle avatar"
                >
                  <div className="w-10 rounded-full">
                    <Image alt="Avatar" src={user.avatar} />
                  </div>
                </button>
                <ul
                  tabIndex={0}
                  className="menu menu-sm dropdown-content bg-base-100 rounded-box mt-3 w-52 p-2 shadow"
                >
                  <li>
                    <a className="justify-between">
                      Profile
                      <span className="badge">New</span>
                    </a>
                  </li>
                  <li>
                    <a>Settings</a>
                  </li>
                  <li>
                    <button onClick={() => setUser(null)}>Logout</button>
                  </li>
                </ul>
              </div>
            ) : (
              <button
                onClick={() =>
                  setUser({
                    name: "John",
                    avatar:
                      "https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp",
                  })
                }
                className="btn btn-primary"
              >
                Sign in
              </button>
            )}
          </div>
        </nav>
      </header>

      {/* Hero mit Login-Card */}
      <main className="flex-1 flex items-center justify-center px-6">
        <section
          className="hero min-h-[70vh] rounded-lg overflow-hidden w-full"
          style={{
            backgroundImage:
              "url('https://img.daisyui.com/images/stock/photo-1507358522600-9f71e620c44e.webp')",
            backgroundSize: "cover",
            backgroundPosition: "center",
          }}
        >
          {/* Overlay */}
          <div className="hero-overlay bg-black/50" />

          {/* Content */}
          <div className="hero-content flex-col lg:flex-row-reverse gap-10 z-10">
            {/* Login-Card */}
            <div className="card bg-base-100 w-full max-w-sm shrink-0 shadow-2xl">
              <div className="card-body">
                <form
                  className="space-y-4"
                  onSubmit={(e) => e.preventDefault()}
                >
                  <div className="form-control">
                    <label className="label">
                      <span className="label-text">Email</span>
                    </label>
                    <input
                      type="email"
                      className="input input-bordered"
                      placeholder="Email"
                      required
                    />
                  </div>

                  <div className="form-control">
                    <label className="label">
                      <span className="label-text">Password</span>
                    </label>
                    <input
                      type="password"
                      className="input input-bordered"
                      placeholder="Password"
                      required
                    />
                  </div>

                  <div className="flex items-center justify-between">
                    <a className="link link-hover text-sm">Forgot password?</a>
                    <button className="btn btn-neutral" type="submit">
                      Login
                    </button>
                  </div>
                </form>
              </div>
            </div>

            {/* Hero Text */}
            <div className="text-center lg:text-left text-white max-w-lg">
              <h1 className="text-5xl font-bold">Welcome back!</h1>
              <p className="py-6">
                Sign in to continue and manage your tasks smartly with{" "}
                <span className="font-bold">smartTask</span>.
              </p>
              <Link href="/register" className="btn btn-primary mr-3">
                Get Started
              </Link>
              <Link href="/login" className="btn btn-ghost">
                Create Account
              </Link>
            </div>
          </div>
        </section>
      </main>

      {/* Footer */}
      <footer className="py-6 px-6 flex gap-6 flex-wrap items-center justify-center bg-base-100">
        <a
          className="flex items-center gap-2 hover:underline"
          href="https://nextjs.org/learn"
          target="_blank"
          rel="noopener noreferrer"
        >
          <Image src="/file.svg" alt="File icon" width={16} height={16} />
          Learn
        </a>
        <a
          className="flex items-center gap-2 hover:underline"
          href="https://vercel.com/templates"
          target="_blank"
          rel="noopener noreferrer"
        >
          <Image src="/window.svg" alt="Window icon" width={16} height={16} />
          Examples
        </a>
        <a
          className="flex items-center gap-2 hover:underline"
          href="https://nextjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          <Image src="/globe.svg" alt="Globe icon" width={16} height={16} />
          Go to nextjs.org →
        </a>
      </footer>
    </div>
  );
}
