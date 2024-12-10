"use client";

import {
  createContext,
  Dispatch,
  SetStateAction,
  useContext,
  useState,
  ReactNode,
} from "react";

export const AuthContext = createContext(
  {} as {
    isLoggedIn: boolean;
    setIsLoggedIn: Dispatch<SetStateAction<boolean>>;
  }
);

export default function AuthContextProvider({
  children,
  auth,
}: {
  children: ReactNode;
  auth: string | undefined;
}) {
  const [isLoggedIn, setIsLoggedIn] = useState(!!auth);

  return (
    <AuthContext.Provider
      value={{
        isLoggedIn,
        setIsLoggedIn,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthContextProvider");
  }
  return context;
}
