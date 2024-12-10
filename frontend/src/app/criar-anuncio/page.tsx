"use client";

import { useState } from "react";
import Image from "next/image";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ChevronLeft, ChevronRight } from "lucide-react";

const rjCities = [
  { id: "1", name: "Rio de Janeiro" },
  { id: "2", name: "Niterói" },
  { id: "3", name: "São Gonçalo" },
  { id: "4", name: "Duque de Caxias" },
  { id: "5", name: "Nova Iguaçu" },
  { id: "6", name: "Belford Roxo" },
  { id: "7", name: "São João de Meriti" },
  { id: "8", name: "Petrópolis" },
  { id: "9", name: "Volta Redonda" },
  { id: "10", name: "Campos dos Goytacazes" },
  { id: "11", name: "Magé" },
  { id: "12", name: "Itaboraí" },
  { id: "13", name: "Macaé" },
  { id: "14", name: "Cabo Frio" },
  { id: "15", name: "Nova Friburgo" },
  { id: "16", name: "Barra Mansa" },
  { id: "17", name: "Angra dos Reis" },
  { id: "18", name: "Teresópolis" },
  { id: "19", name: "Mesquita" },
  { id: "20", name: "Nilópolis" },
];

export default function CreateAdPage() {
  const [adType, setAdType] = useState("oferta");
  const [photos, setPhotos] = useState<File[]>([]);
  const [previews, setPreviews] = useState<string[]>([]);
  const [currentPreview, setCurrentPreview] = useState(0);
  const [location, setLocation] = useState("");

  const handlePhotoUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      const files = Array.from(event.target.files).splice(0, 6);

      setPhotos(files);
      const newPreviews = files.map((file) => URL.createObjectURL(file));
      setPreviews((prev) => prev.concat(newPreviews));
      setCurrentPreview(0);
    }
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    // Here you would typically send the form data to your backend
    console.log("Form submitted");
  };

  const nextPreview = () => {
    setCurrentPreview((prev) => (prev + 1) % previews.length);
  };

  const prevPreview = () => {
    setCurrentPreview((prev) => (prev - 1 + previews.length) % previews.length);
  };

  return (
    <div className="container mx-auto py-8">
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl font-bold">Criar Anúncio</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-6">
            {previews.length > 0 && (
              <div className="relative aspect-video w-full overflow-hidden rounded-lg max-w-[500px] mx-auto">
                <Image
                  src={previews[currentPreview]}
                  alt={`Preview ${currentPreview + 1}`}
                  fill
                  className="object-cover"
                />
                {previews.length > 1 && (
                  <div className="absolute inset-0 flex items-center justify-between p-4">
                    <Button
                      type="button"
                      variant="outline"
                      size="icon"
                      onClick={prevPreview}
                    >
                      <ChevronLeft className="h-4 w-4" />
                    </Button>
                    <Button
                      type="button"
                      variant="outline"
                      size="icon"
                      onClick={nextPreview}
                    >
                      <ChevronRight className="h-4 w-4" />
                    </Button>
                  </div>
                )}
                <div className="absolute bottom-4 left-1/2 -translate-x-1/2 bg-black/50 text-white px-2 py-1 rounded-full text-sm">
                  {currentPreview + 1} / {previews.length}
                </div>
              </div>
            )}

            <div className="space-y-2">
              <Label htmlFor="photos">Fotos</Label>
              <Input
                id="photos"
                type="file"
                multiple
                onChange={handlePhotoUpload}
                accept="image/png, image/jpg, image/jpeg, image/webp"
              />

              {photos.length > 6 && (
                <p className="text-red-500">
                  Você pode enviar no máximo 6 imagens.
                </p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="description">Descrição Detalhada</Label>
              <Textarea
                id="description"
                placeholder="Descreva seu an����ncio em detalhes"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="adType">Tipo de Anúncio</Label>
              <Select onValueChange={setAdType} defaultValue={adType}>
                <SelectTrigger>
                  <SelectValue placeholder="Selecione o tipo de anúncio" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="busca">Busca</SelectItem>
                  <SelectItem value="oferta">Oferta</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="price">Preço</Label>
              <Input
                id="price"
                type="number"
                placeholder="Preço (se aplicável)"
                disabled={adType === "busca"}
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="contact">Informações de Contato</Label>
              <Input
                id="contact"
                placeholder="Seu email ou telefone"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="adType">Cidade</Label>
              <Select onValueChange={setLocation} defaultValue={location}>
                <SelectTrigger>
                  <SelectValue placeholder="Selecione uma cidade" />
                </SelectTrigger>
                <SelectContent>
                  {rjCities.map((city) => (
                    <SelectItem key={city.id} value={city.id}>
                      {city.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="category">Categoria</Label>
              <Select>
                <SelectTrigger>
                  <SelectValue placeholder="Selecione uma categoria" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="livros">Livros</SelectItem>
                  <SelectItem value="roupas">Roupas</SelectItem>
                  <SelectItem value="moveis">Móveis</SelectItem>
                  <SelectItem value="aulas">Aulas Particulares</SelectItem>
                  <SelectItem value="outros">Outros</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <Button type="submit" className="w-full">
              Criar Anúncio
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
