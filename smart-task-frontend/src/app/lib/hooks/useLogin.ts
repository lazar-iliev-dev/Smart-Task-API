"use client";
import { useMutation } from "@tanstack/react-query";
import { apiFetch } from "../api";

type LoginReq = { email: string; password: string };
type LoginRes = { token: string; user: { id: string; email: string; avatarUrl?: string } };

export function useLogin() {
  return useMutation<LoginRes, Error, LoginReq>({
    mutationFn: (data) =>
      apiFetch<LoginRes>("/auth/login", {
        method: "POST",
        body: JSON.stringify(data),
      }),
    onSuccess: (data) => {
      localStorage.setItem("token", data.token);
      localStorage.setItem("user", JSON.stringify(data.user));
      window.location.href = "/dashboard"; // redirect nach Login
    },
  });
}
