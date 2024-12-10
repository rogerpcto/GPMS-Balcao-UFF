"use server";

import { setAcessToken } from "@/lib/actions/handle-acess-token";
// import { getErrorMessage } from "@/lib/helpers/error-handling";

export async function login(email: string, password: string) {
  setAcessToken("fake-access-token");
}
