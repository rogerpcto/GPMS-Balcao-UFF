"use client";

import {
  MessageCircle,
  PlusCircle,
  Search,
  ShoppingBag,
  User,
  Shapes,
} from "lucide-react";
import Link from "next/link";
import { Input } from "../../../../../balcao-uff/src/components/ui/input";
import { Button } from "../../../../../balcao-uff/src/components/ui/button";
import { useState } from "react";

export function Header() {
  const [searchTerm, setSearchTerm] = useState("");
  const [isLoggedIn, setIsLoggedIn] = useState(true);

  return (
    <header className="sticky top-0 z-50 w-full border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
      <div className="container flex h-14 items-center">
        <nav className="flex items-center space-x-4 lg:space-x-6 mx-6">
          <Link
            href="/"
            className="text-sm font-medium transition-colors hover:text-primary"
          >
            <ShoppingBag className="h-4 w-4 mr-2 inline-block" />
            Anúncios
          </Link>
          <Link
            href="/criar-anuncio"
            className="text-sm font-medium transition-colors hover:text-primary"
          >
            <PlusCircle className="h-4 w-4 mr-2 inline-block" />
            Criar um anúncio
          </Link>
          <Link
            href="/chat"
            className="text-sm font-medium transition-colors hover:text-primary"
          >
            <MessageCircle className="h-4 w-4 mr-2 inline-block" />
            Chat
          </Link>
          <Link
            href="/meus-anuncios"
            className="text-sm font-medium transition-colors hover:text-primary"
          >
            <Shapes className="h-4 w-4 mr-2 inline-block" />
            Meus anúncios
          </Link>
          {isLoggedIn ? (
            <Link href="/perfil/1">
              <Button size="sm" variant="ghost">
                <User className="h-4 w-4" />
              </Button>
            </Link>
          ) : (
            <Link href="/login">
              <Button size="sm" variant="outline">
                Entrar
              </Button>
            </Link>
          )}
        </nav>
        <div className="flex items-center ml-auto">
          <Input
            type="search"
            placeholder="Buscar produtos ou serviços"
            className="w-[300px] mr-2"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          <Button size="sm" variant="ghost">
            <Search className="h-4 w-4" />
          </Button>
        </div>
      </div>
    </header>
  );
}
