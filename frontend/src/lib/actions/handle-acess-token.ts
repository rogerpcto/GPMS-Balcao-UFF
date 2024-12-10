"use server";

import { cookies } from "next/headers";

export async function setAcessToken(token: string) {
  cookies().set("auth-token", token, {
    httpOnly: true,
    secure: true,
    sameSite: "lax",
    maxAge: 60 * 60 * 24,
  });
}

export async function getAcessToken() {
  return cookies().get("auth-token")?.value;
}
