"use client";

import { useState } from "react";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import Image from "next/image";

// Simulated product data
const products = [
  {
    id: 1,
    name: "Produto",
    image: "/imgs/produto.png",
    date: "2023-07-01",
    location: "São Paulo",
    category: "eletronicos",
    price: "99,99",
  },
  {
    id: 2,
    name: "Produto",
    image: "/imgs/produto.png",
    date: "2023-07-02",
    location: "Rio de Janeiro",
    price: "99,99",
  },
  {
    id: 3,
    name: "Produto",
    image: "/imgs/produto.png",
    date: "2023-07-03",
    location: "Belo Horizonte",
    price: "99,99",
  },
  {
    id: 4,
    name: "Produto",
    image: "/imgs/produto.png",
    date: "2023-07-01",
    location: "São Paulo",
    category: "eletronicos",
    price: "99,99",
  },
  {
    id: 5,
    name: "Produto",
    image: "/imgs/produto.png",
    date: "2023-07-02",
    location: "Rio de Janeiro",
    price: "99,99",
  },
  {
    id: 6,
    name: "Produto",
    image: "/imgs/produto.png",
    date: "2023-07-03",
    location: "Belo Horizonte",
    price: "99,99",
  },
  {
    id: 7,
    name: "Produto",
    image: "/imgs/produto.png",
    date: "2023-07-01",
    location: "São Paulo",
    category: "eletronicos",
    price: "99,99",
  },
  {
    id: 8,
    name: "Produto",
    image: "/imgs/produto.png",
    date: "2023-07-02",
    location: "Rio de Janeiro",
    price: "99,99",
  },
  {
    id: 9,
    name: "Produto",
    price: "99,99",
    image: "/imgs/produto.png",
    date: "2023-07-03",
    location: "Belo Horizonte",
  },
  {
    id: 10,
    name: "Produto",
    price: "99,99",
    image: "/imgs/produto.png",
    date: "2023-07-01",
    location: "São Paulo",
    category: "eletronicos",
  },
  {
    id: 11,
    name: "Produto",
    price: "99,99",
    image: "/imgs/produto.png",
    date: "2023-07-02",
    location: "Rio de Janeiro",
  },
  {
    id: 12,
    name: "Produto",
    price: "99,99",
    image: "/imgs/produto.png",
    date: "2023-07-03",
    location: "Belo Horizonte",
  },
  {
    id: 13,
    name: "Produto",
    price: "99,99",
    image: "/imgs/produto.png",
    date: "2023-07-01",
    location: "São Paulo",
    category: "eletronicos",
  },
  {
    id: 14,
    name: "Produto",
    price: "99,99",
    image: "/imgs/produto.png",
    date: "2023-07-02",
    location: "Rio de Janeiro",
  },
  {
    id: 15,
    name: "Produto",
    price: "99,99",
    image: "/imgs/produto.png",
    date: "2023-07-03",
    location: "Belo Horizonte",
  },

  // Add more products as needed
];

export default function Home() {
  const [searchTerm, setSearchTerm] = useState("");
  const [category, setCategory] = useState("");
  const [location, setLocation] = useState("");
  const [priceRange, setPriceRange] = useState("");
  const [showFilters, setShowFilters] = useState(false);

  const filteredProducts = products.filter(
    (product) =>
      product.name.toLowerCase().includes(searchTerm.toLowerCase()) &&
      (category === "" || product.category === category) &&
      (location === "" || product.location === location)
    // Price range filter would be implemented here if we had price data
  );

  return (
    <div className="flex flex-col min-h-screen">
      <main className="flex-1 container py-6 mx-auto">
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-2xl font-bold">Produtos e Serviços</h1>
          <Button
            onClick={() => setShowFilters(!showFilters)}
            variant="outline"
          >
            {showFilters ? "Esconder Filtros" : "Mostrar Filtros"}
          </Button>
        </div>

        {showFilters && (
          <div className="flex space-x-4 mb-6">
            <Select value={category} onValueChange={setCategory}>
              <SelectTrigger className="w-[180px]">
                <SelectValue placeholder="Categoria" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="eletronicos">Eletrônicos</SelectItem>
                <SelectItem value="moveis">Móveis</SelectItem>
                <SelectItem value="roupas">Roupas</SelectItem>
              </SelectContent>
            </Select>
            <Select value={location} onValueChange={setLocation}>
              <SelectTrigger className="w-[180px]">
                <SelectValue placeholder="Localização" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="sao-paulo">São Paulo</SelectItem>
                <SelectItem value="rio-de-janeiro">Rio de Janeiro</SelectItem>
                <SelectItem value="belo-horizonte">Belo Horizonte</SelectItem>
              </SelectContent>
            </Select>
            <Select value={priceRange} onValueChange={setPriceRange}>
              <SelectTrigger className="w-[180px]">
                <SelectValue placeholder="Faixa de Preço" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="0-100">R$ 0 - R$ 100</SelectItem>
                <SelectItem value="100-500">R$ 100 - R$ 500</SelectItem>
                <SelectItem value="500+">R$ 500+</SelectItem>
              </SelectContent>
            </Select>
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredProducts.map((product) => (
            <Link href={`/anuncio/${product.id}`} key={product.id}>
              <Card className="overflow-hidden">
                <Image
                  src={product.image}
                  alt={product.name}
                  className="w-full h-48 object-cover"
                  width={500}
                  height={500}
                  priority
                />
                <CardContent className="p-4">
                  <div className="flex justify-between items-center">
                    <h2 className="text-lg font-semibold">{product.name}</h2>
                    <span className="text-xl font-bold">
                      R$ {product.price}
                    </span>
                  </div>
                  <p className="text-sm text-gray-500">
                    Anunciado em: {product.date}
                  </p>
                  <p className="text-sm text-gray-500">
                    Localização: {product.location}
                  </p>
                </CardContent>
              </Card>
            </Link>
          ))}
        </div>
      </main>
    </div>
  );
}
