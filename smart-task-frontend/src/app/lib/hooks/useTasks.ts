"use client";
import { useQuery } from "@tanstack/react-query";
import { apiFetch } from "../api";
import { TaskItem } from "@/app/types/TaskItem";

export function useTasks() {
  return useQuery({
    queryKey: ["tasks"],
    queryFn: () => apiFetch<TaskItem[]>("/tasks"),
  });
}
