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
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useState } from "react";
import { useAuth } from "@/context/auth-context";
import { getAds } from "@/data-access/advertisement";
import { useProducts } from "@/context/products-context";

export function Header() {
  const [searchTerm, setSearchTerm] = useState("");
  const { isLoggedIn, user } = useAuth();

    const { setProducts } = useProducts();
  

    const searchProducts = () => {

      if(!searchTerm) return;
  
      const searchParams = new URLSearchParams({ consulta: searchTerm });
  
      getAds(searchParams?.toString()).then(res=> {
        if(!res) return;
        setProducts(res);
      })
    }

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
            href="/meus-anuncios"
            className="text-sm font-medium transition-colors hover:text-primary"
          >
            <Shapes className="h-4 w-4 mr-2 inline-block" />
            Meus anúncios
          </Link>
        </nav>

        <div className="flex items-center ml-auto">
          <Input
            type="search"
            placeholder="Buscar produtos ou serviços"
            className="w-[300px] mr-2"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          <Button size="sm" variant="ghost" onClick={() => searchProducts()}>
            <Search className="h-4 w-4" />
          </Button>
        </div>

        <nav className="flex items-center space-x-4 lg:space-x-6 mx-6">
          {isLoggedIn ? (
            <Link href={`/perfil/${user?.nameid}`}>
              <Button size="sm" variant="ghost">
                <User className="h-4 w-4" />
              </Button>
            </Link>
          ) : (
            <>
              <Link
                href="/Cadastro"
                className="text-sm font-medium transition-colors hover:text-primary"
              >
                Criar conta
              </Link>

              <Link href="/login">
                <Button size="sm" variant="outline">
                  Entrar
                </Button>
              </Link>
            </>
          )}
        </nav>
      </div>
    </header>
  );
}
