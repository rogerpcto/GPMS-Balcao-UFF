"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Badge } from "@/components/ui/badge";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
  DialogFooter,
} from "@/components/ui/dialog";
import { Star, MessageCircle, Info } from "lucide-react";
import { useRouter } from "next/navigation";

// Mock data for demonstration
const myAds = [
  {
    id: 1,
    title: "Sofá em ótimo estado",
    status: "active",
    type: "oferta",
    price: 500,
    category: "móveis",
  },
  {
    id: 2,
    title: "Procuro livros de programação",
    status: "active",
    type: "busca",
    category: "livros",
  },
  {
    id: 3,
    title: "Aulas de inglês",
    status: "invalid",
    type: "oferta",
    price: 50,
    category: "aulas",
  },
];

const participatedAds = [
  {
    id: 4,
    title: "iPhone usado",
    status: "completed",
    type: "oferta",
    price: 1500,
    category: "eletrônicos",
  },
  {
    id: 5,
    title: "Bicicleta mountain bike",
    status: "completed",
    type: "oferta",
    price: 800,
    category: "esportes",
  },
];

type Ad = {
  id: number;
  title: string;
  status: string;
  type: string;
  price?: number;
  category: string;
};

const StarRating = ({
  rating,
  setRating,
}: {
  rating: number;
  setRating: (rating: number) => void;
}) => {
  return (
    <div className="flex items-center space-x-1">
      {[1, 2, 3, 4, 5].map((star) => (
        <button
          key={star}
          onClick={() => setRating(star)}
          className={`text-2xl ${
            star <= rating ? "text-yellow-400" : "text-gray-300"
          }`}
          aria-label={`Rate ${star} stars out of 5`}
        >
          ★
        </button>
      ))}
    </div>
  );
};

const AdCard = ({ ad }: { ad: Ad }) => {
  const [isInfoDialogOpen, setIsInfoDialogOpen] = useState(false);
  const [isRatingDialogOpen, setIsRatingDialogOpen] = useState(false);
  const [rating, setRating] = useState(0);

  const { push } = useRouter();

  const handleRatingSubmit = () => {
    // Here you would typically send the rating to your backend
    console.log(`Submitted rating of ${rating} stars for ad ${ad.id}`);
    setIsRatingDialogOpen(false);
    setRating(0);
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center justify-between">
          {ad.title}
          <Badge
            variant={
              ad.status === "active"
                ? "default"
                : ad.status === "completed"
                ? "secondary"
                : "destructive"
            }
          >
            {ad.status === "active"
              ? "Ativo"
              : ad.status === "completed"
              ? "Finalizado"
              : "Inválido"}
          </Badge>
        </CardTitle>
        <CardDescription>
          {ad.category} - {ad.type === "oferta" ? "Oferta" : "Busca"}
        </CardDescription>
      </CardHeader>
      <CardContent className="h-12">
        {ad.price && (
          <p className="text-2xl font-bold">R$ {ad.price.toFixed(2)}</p>
        )}
      </CardContent>
      <CardFooter className="flex justify-between mt-auto">
        <Dialog open={isInfoDialogOpen} onOpenChange={setIsInfoDialogOpen}>
          <DialogTrigger asChild>
            <Button variant="outline" size="sm">
              <Info className="w-4 h-4 mr-2" />
              Detalhes
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>{ad.title}</DialogTitle>
              <DialogDescription>
                Categoria: {ad.category}
                <br />
                Tipo: {ad.type === "oferta" ? "Oferta" : "Busca"}
                <br />
                {ad.price && `Preço: R$ ${ad.price.toFixed(2)}`}
                <br />
                Status:{" "}
                {ad.status === "active"
                  ? "Ativo"
                  : ad.status === "completed"
                  ? "Finalizado"
                  : "Inválido"}
              </DialogDescription>
            </DialogHeader>
          </DialogContent>
        </Dialog>
        {ad.status === "active" && (
          <Button onClick={() => push("/chat")} variant="secondary" size="sm">
            <MessageCircle className="w-4 h-4 mr-2" />
            Chat
          </Button>
        )}
        {ad.status === "invalid" && (
          <Dialog
            open={isRatingDialogOpen}
            onOpenChange={setIsRatingDialogOpen}
          >
            <DialogTrigger asChild>
              <Button variant="secondary" size="sm">
                <Star className="w-4 h-4 mr-2" />
                Avaliar
              </Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>Avaliar Anúncio</DialogTitle>
                <DialogDescription>
                  Por favor, avalie este anúncio de 1 a 5 estrelas.
                </DialogDescription>
              </DialogHeader>
              <div className="py-4">
                <StarRating rating={rating} setRating={setRating} />
              </div>
              <DialogFooter>
                <Button onClick={handleRatingSubmit} disabled={rating === 0}>
                  Enviar Avaliação
                </Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
        )}
      </CardFooter>
    </Card>
  );
};

export default function MeusAnuncios() {
  return (
    <div className="container mx-auto py-8">
      <h1 className="text-3xl font-bold mb-6">Meus Anúncios</h1>
      <Tabs defaultValue="my-ads">
        <TabsList className="grid w-full grid-cols-2">
          <TabsTrigger value="my-ads">Meus Anúncios</TabsTrigger>
          <TabsTrigger value="participated">Anúncios Participados</TabsTrigger>
        </TabsList>
        <TabsContent value="my-ads">
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {myAds.map((ad) => (
              <AdCard key={ad.id} ad={ad} />
            ))}
          </div>
        </TabsContent>
        <TabsContent value="participated">
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {participatedAds.map((ad) => (
              <AdCard key={ad.id} ad={ad} />
            ))}
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
