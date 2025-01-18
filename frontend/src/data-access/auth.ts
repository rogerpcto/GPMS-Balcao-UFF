"use server";

import { getErrorMessage } from "@/lib/helpers/error-handling";

export async function login(email: string, senha: string) {
  try {
    const response = await fetch(
      `${process.env.NEXT_PUBLIC_API_URL}/Usuarios/Login?email=${email}&senha=${senha}`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ email, senha }),
      }
    );

    return await response.json();
  } catch (error) {
    alert(getErrorMessage(error));
    return null;
  }
}

export async function register(input: {
  email: string;
  senha: string;
  nome: string;
}) {
  try {
    const response = await fetch(
      `${process.env.NEXT_PUBLIC_API_URL}/Usuarios`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(input),
      }
    );

    return await response.json();
  } catch (error) {
    alert(getErrorMessage(error));
    return null;
  }
}
