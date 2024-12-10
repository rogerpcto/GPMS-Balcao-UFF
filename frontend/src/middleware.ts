import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const protectedRoutes = ["/chat", "/criar-anuncio", "/meus-anuncios"];
  const publicRoutes = ["/login", "/cadastro"];

  const token = request.cookies.get("auth-token")?.value;

  if (protectedRoutes.includes(request.nextUrl.pathname) && !token) {
    return NextResponse.redirect(new URL("/login", request.url));
  }

  if (publicRoutes.includes(request.nextUrl.pathname) && token) {
    return NextResponse.redirect(new URL("/", request.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: ["/chat", "/criar-anuncio", "/meus-anuncios", "/login", "/cadastro"],
};
