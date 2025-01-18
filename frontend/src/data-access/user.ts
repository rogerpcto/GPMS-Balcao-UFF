"use server";

import { getErrorMessage } from "@/lib/helpers/error-handling";

export async function getProfile(id: string) {
  
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Usuarios/${id}`);
    return await response.json();
    
  } catch (error) {
    alert(getErrorMessage(error));
    return null;
  }
}

