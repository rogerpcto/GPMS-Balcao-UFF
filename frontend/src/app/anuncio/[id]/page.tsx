"use client";

import Link from "next/link";
import { MapPin, Star } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { ImageCarousel } from "@/components/carousel";

interface ProductImage {
  src: string;
  alt: string;
}

interface Product {
  id: string;
  name: string;
  price: number;
  location: string;
  images: ProductImage[];
}

interface Advertiser {
  id: string;
  name: string;
  reputation: number;
  profileUrl: string;
}

export default function ProductView() {
  const product: Product = {
    id: "bike-123",
    name: "Bicicleta Mountain Bike Profissional",
    price: 2500,
    location: "São Paulo, SP",
    images: [
      {
        src: "/imgs/produto.png",
        alt: "Bicicleta vista frontal",
      },
      {
        src: "/imgs/produto.png",
        alt: "Bicicleta vista lateral",
      },
      {
        src: "/imgs/produto.png",
        alt: "Bicicleta vista traseira",
      },
    ],
  };

  const images = product.images.map((image) => image.src);

  const advertiser: Advertiser = {
      id: "joao-123",
      name: "João Ciclista",
      reputation: 4.8,
      profileUrl: "/perfil-joao"
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="grid md:grid-cols-2 gap-8">
        <ImageCarousel images={images} />
        <div>
          <h1 className="text-3xl font-bold mb-4">{product.name}</h1>
          <p className="text-2xl font-semibold mb-4">
            R$ {product.price.toLocaleString("pt-BR")}
          </p>
          <div className="flex items-center mb-6">
            <MapPin className="h-5 w-5 mr-2 text-muted-foreground" />
            <span className="text-muted-foreground">{product.location}</span>
          </div>
          <Card>
            <CardContent className="p-6">
              <h2 className="text-xl font-semibold mb-4">
                Informações do Anunciante
              </h2>
              <div className="flex items-center justify-between mb-4">
                <span className="font-medium">{advertiser.name}</span>
                <div className="flex items-center">
                  <Star className="h-5 w-5 text-yellow-400 mr-1" />
                  <span>{advertiser.reputation.toFixed(1)}</span>
                </div>
              </div>
              <Link href="/perfil/joao-123" passHref>
                <Button variant="outline" className="w-full">
                  Ver perfil do anunciante
                </Button>
              </Link>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
