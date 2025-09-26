"use client";

import { ReactNode, useState } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";

export default function ReactQueryProvider({ children }: { children: ReactNode }) {
  // create QueryClient only once per render root
  const [queryClient] = useState(() => new QueryClient());

  return (
    <QueryClientProvider client={queryClient}>
      {children}
      {/* optional: entferne in Produktion */}
      <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  );
}

