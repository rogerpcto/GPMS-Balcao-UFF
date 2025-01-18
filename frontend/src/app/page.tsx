"use client";

import { useEffect, useState } from "react";
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
import { getAds, getCategoria } from "@/data-access/advertisement";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import { useProducts } from "@/context/products-context";

export interface Product {
  id: number
  titulo: string
  descricao: string
  ativo: boolean
  nota: number
  dataCriacao: string
  preco: number
  quantidade: number
  proprietario: Proprietario
  imagem: Imagem[]
}

export interface Proprietario {
  id: number
  nome: string
  email: string
  nota: number
  perfil: string
}

export interface Imagem {
  id: number
  url: string
}

export function formatDate(inputDate: string) {
  const date = new Date(inputDate);

  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0'); // Os meses começam do 0
  const year = date.getFullYear();

  return `${day}/${month}/${year}`;
}


export default function Home() {
  const [showFilters, setShowFilters] = useState(false);

  const [precoMinimo, setPrecoMinimo] = useState('')
  const [precoMaximo, setPrecoMaximo] = useState('')
  const [dataMinima, setDataMinima] = useState('')
  const [dataMaxima, setDataMaxima] = useState('')
  const [categories, setCategories] = useState([]);
  const [category, setCategory] = useState('');

  const { products, setProducts } = useProducts();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()

    const searchParams = new URLSearchParams(
      Object.entries({ precoMinimo, precoMaximo, dataMinima, dataMaxima, categoria: category })
        .filter(([_, value]) => value !== '') 
    );

    getAds(searchParams?.toString()).then(res=> {
      if(!res) return;
      setProducts(res);
    })
  }

  useEffect(() => { 
    const fetchData = async () => {
      getAds().then(res => {
        setProducts(res);
      });
    };

    const fetchCategories = async () => {
      getCategoria().then(res => {
        if(!res) return;
        setCategories(res)
      });
    };

    fetchData();
    fetchCategories();
  }, []);


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
          <form onSubmit={handleSubmit} className="flex flex-wrap items-end gap-4 mb-8">
            <div className="flex-1 min-w-[120px]">
              <Label htmlFor="valorMinimo" className="mb-2 block">Catregoria</Label>
              <Select onValueChange={(value) => {
                setCategory(value)
              }}>
                    <SelectTrigger>
                      <SelectValue placeholder="Selecione uma categoria" />
                    </SelectTrigger>
                    <SelectContent>
                    {categories.map((cat) => (
                        <SelectItem key={cat} value={cat}>
                          {cat}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
            </div>
            <div className="flex-1 min-w-[120px]">
              <Label htmlFor="valorMinimo" className="mb-2 block">Valor Mínimo</Label>
              <Input
                id="valorMinimo"
                type="number"
                placeholder="0,00"
                value={precoMinimo}
                onChange={(e) => setPrecoMinimo(e.target.value)}
              />
            </div>
            <div className="flex-1 min-w-[120px]">
              <Label htmlFor="valorMaximo" className="mb-2 block">Valor Máximo</Label>
              <Input
                id="valorMaximo"
                type="number"
                placeholder="1000,00"
                value={precoMaximo}
                onChange={(e) => setPrecoMaximo(e.target.value)}
              />
            </div>
            <div className="flex-1 min-w-[120px]">
              <Label htmlFor="dataMinima" className="mb-2 block">Data Mínima</Label>
              <Input
                id="dataMinima"
                type="date"
                value={dataMinima}
                onChange={(e) => setDataMinima(e.target.value)}
              />
            </div>
            <div className="flex-1 min-w-[120px]">
              <Label htmlFor="dataMaxima" className="mb-2 block">Data Máxima</Label>
              <Input
                id="dataMaxima"
                type="date"
                value={dataMaxima}
                onChange={(e) => setDataMaxima(e.target.value)}
              />
            </div>
            <Button type="submit" className="ml-auto">
              Aplicar Filtros
            </Button>
          </form>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {products.length > 0 && products.map((product) => (
            <Link href={`/anuncio/${product.id}`} key={product.id}>
              <Card className="overflow-hidden">
                  <Image
                    src={product?.imagem[0]?.url ? `http://localhost:5037/Imagens?fileName=${product?.imagem[0]?.url}` : `/imgs/produto.png`}
                    alt={product.titulo}
                    className="w-full h-48 object-cover"
                    width={500}
                    height={500}
                    priority
                  />
                <CardContent className="p-4">
                  <div className="flex justify-between items-center">
                    <h2 className="text-lg font-semibold">{product.titulo}</h2>
                    <span className="text-xl font-bold">
                      R$ {product.preco}
                    </span>
                  </div>
                  <p className="text-sm text-gray-500">
                    Anunciado em: {formatDate(product.dataCriacao)}
                  </p>
                  <p className="text-sm text-gray-500">
                    Localização: {product.localizacao}
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
