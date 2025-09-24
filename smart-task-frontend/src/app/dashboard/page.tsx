"use client";
import { useTasks } from "../lib/hooks/useTasks";
import Image from "next/image";
import Link from "next/link";
import { useEffect, useState } from "react";
import { TaskItem } from "../types/TaskItem";

export default function DashboardPage() {
  const { data, isLoading, error } = useTasks();
  const [user, setUser] = useState<{ email: string; avatarUrl?: string } | null>(null);

  useEffect(() => {
    const stored = localStorage.getItem("user");
    if (stored) setUser(JSON.parse(stored));
  }, []);

  if (!localStorage.getItem("token")) {
    if (typeof window !== "undefined") {
      window.location.href = "/login";
    }
    return null;
  }

  if (isLoading) return <p className="p-6">Loading tasks...</p>;
  if (error) return <p className="p-6 text-error">Error: {String(error)}</p>;

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Dashboard</h1>
        {user && (
          <div className="flex items-center gap-2">
            <span>{user.email}</span>
            <div className="avatar">
              <div className="w-10 rounded-full">
                <Image src={user.avatarUrl || "https://i.pravatar.cc/100"} alt="avatar" />
              </div>
            </div>
          </div>
        )}
      </div>

      <div className="grid gap-4">
        {data?.length ? (
          data.map((task: TaskItem) => (
            <div key={task.id} className="card bg-base-100 shadow p-4">
              <h2 className="text-lg font-semibold">{task.title}</h2>
              <p>{task.description}</p>
              <Link href={`/dashboard/tasks/${task.id}`} className="link mt-2">
                View Details
              </Link>
            </div>
          ))
        ) : (
          <p>No tasks found.</p>
        )}
      </div>
    </div>
  );
}
/** 
 * import { useTasks } from "@/lib/hooks/useTasks";
import { TaskItem } from "@/app/types/TaskItem";
import Link from "next/link";

export default function DashboardPage() {
  const { data, isLoading, error } = useTasks();

  if (isLoading) return <p>Loading...</p>;
  if (error) return <p className="text-error">Error loading tasks</p>;

  return (
    <div className="grid gap-4">
      {data && data.length > 0 ? (
        data.map((task: TaskItem) => (
          <div key={task.id} className="card bg-base-100 shadow p-4">
            <h2 className="text-lg font-semibold">{task.title}</h2>
            <p>{task.description}</p>
            <Link href={`/dashboard/tasks/${task.id}`} className="link mt-2">
              View Details
            </Link>
          </div>
        ))
      ) : (
        <p>No tasks found.</p>
      )}
    </div>
  );
}
 */