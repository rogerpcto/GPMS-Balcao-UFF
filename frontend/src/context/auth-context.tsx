"use client";

import {
  createContext,
  Dispatch,
  SetStateAction,
  useContext,
  useState,
  ReactNode,
} from "react";

import { jwtDecode } from "jwt-decode";

type User = {
  unique_name: string
  email: string
  nameid: string
  role: string
  nbf: number
  exp: number
  iat: number
} | null;

export const AuthContext = createContext(
  {} as {
    isLoggedIn: boolean;
    setIsLoggedIn: Dispatch<SetStateAction<boolean>>;
    user: User;
    updateUser: (token: string) => void;
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


  const updateUser = (token: string) => {
    const authToken = token ?? auth;
    const decodedUser: User = authToken ? jwtDecode(authToken) : null;
    setUser(decodedUser);
  };

  const initialUser = (auth ? jwtDecode(auth) : null) as User;

  const [user, setUser] = useState<User>(initialUser);

  return (
    <AuthContext.Provider
      value={{
        isLoggedIn,
        setIsLoggedIn,
        user,
        updateUser
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
