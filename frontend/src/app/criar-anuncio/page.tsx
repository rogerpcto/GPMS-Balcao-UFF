"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";

import { useEffect, useState } from "react";
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
import { Controller, useForm } from "react-hook-form";
import { createAd, getCategoria, getTipoAnuncio, sendImages } from "@/data-access/advertisement";

export const rjCities = [
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

const createAdSchema = z.object({
  titulo: z.string(),
  descricao: z.string(),
  preco: z.string().optional(), 
  quantidade: z.string(),
  cidade: z.string(),
  bairro: z.string(),
  categoria: z.string(),
  tipoAnuncio: z.string(),
  contato: z.string(),
}).refine(
  (data) => data.tipoAnuncio !== "OFERTA" || data.preco !== undefined,
  {
    message: "O preço é obrigatório para anúncios que não são do tipo 'OFERTA'.",
    path: ["preco"], 
  }
);

type createAdFormValues = z.infer<typeof createAdSchema>;

export default function CreateAdPage() {
  const [photos, setPhotos] = useState<File>();
  const [previews, setPreviews] = useState<string[]>([]);
  const [currentPreview, setCurrentPreview] = useState(0);
  const [adTypes, setAdTypes] = useState([]);
  const [categories, setCategories] = useState([]);

  const handlePhotoUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      const files = event.target.files[0];

      setPhotos(files);
      const newPreviews =  URL.createObjectURL(files);
      setPreviews((prev) => prev.concat(newPreviews));
      setCurrentPreview(0);
    }
  };

  const nextPreview = () => {
    setCurrentPreview((prev) => (prev + 1) % previews.length);
  };

  const prevPreview = () => {
    setCurrentPreview((prev) => (prev - 1 + previews.length) % previews.length);
  };

  const { control, handleSubmit, watch, reset, formState: { errors } } = useForm<createAdFormValues>({
    resolver: zodResolver(createAdSchema),
    defaultValues: {
      titulo: "",
      descricao: "",
      preco: '',
      quantidade: '1',
      cidade: "",
      bairro: "",
      categoria: "",
      contato: "",
      tipoAnuncio: "OFERTA",
    },
  });

  const onSubmit = async (values: createAdFormValues) => {
    const data = {
      titulo: values.titulo,
      descricao: values.preco,
      preco: parseFloat(values.preco as string), 
      quantidade: Number(values.quantidade),
      localizacao: `${values.bairro} - ${values.cidade}`,
      categoria: values.titulo,
      contato: values.contato,
      tipoAnuncio: values.tipoAnuncio,
    };

    createAd(data).then((res) => {
      if (!res) return;
      console.log("🚀 ~ createAd ~ res:", res)
      
      handleImageSubmit(res.id);
    });
  };

  const handleImageSubmit = (anuncioId: string) => {

    console.log("🚀 ~ handleImageSubmit ~ photos:", photos)
    if (!photos) {
      alert("Por favor, selecione pelo menos uma imagem.");
      return;
    }
    
    console.log("🚀 ~ handleImageSubmit ~ photos:", photos)
    sendImages(anuncioId, (photos as unknown as FileList))
      .then(res => {
        if(!res) return;
        console.log("🚀 ~ Imagem:", res);

        // reset();
        // setPhotos(null);

      });
  };

  useEffect(() => {
    const fetchCategories = async () => {
      getCategoria().then(res => {
        if(!res) return;
        setCategories(res)
      });
    }

    const fetchAdTypes = async () => {
      getTipoAnuncio().then(res => {
        if(!res) return;
          setAdTypes(res);
        });
      }

    fetchCategories();
    fetchAdTypes();
  }, []);

  const adType = watch('tipoAnuncio');
  
  return (
    <div className="container mx-auto py-8">
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl font-bold">Criar Anúncio</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)}  className="space-y-6">
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
                onChange={handlePhotoUpload}
                accept="image/png, image/jpg, image/jpeg, image/webp"
              />

              {/* {photos.length > 1 && (
                <p className="text-red-500">
                  Você pode enviar no máximo 1 imagem.
                </p>
              )} */}
            </div>

            <Controller
              name="titulo"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="titulo">Título</Label>
                  <Input {...field} id="titulo" placeholder="Título do anúncio" />
                  {errors.titulo && <p className="text-red-500">{errors.titulo.message}</p>}
                </div>
              )}
            />

            <Controller
              name="descricao"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="descricao">Descrição Detalhada</Label>
                  <Textarea {...field} id="descricao" placeholder="Descreva seu anúncio em detalhes" />
                  {errors.descricao && <p className="text-red-500">{errors.descricao.message}</p>}
                </div>
              )}
            />

            <Controller
              name="quantidade"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="quantidade">Quantidade</Label>
                  <Input {...field} id="quantidade" type="number" min={1} />
                  {errors.quantidade && <p className="text-red-500">{errors.quantidade.message}</p>}
                </div>
              )}
            />


            <Controller
              name="tipoAnuncio"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="adType">Tipo de Anúncio</Label>
                  <Select onValueChange={field.onChange} defaultValue={"OFERTA"}>
                    <SelectTrigger>
                      <SelectValue placeholder="Selecione o tipo de anúncio" />
                    </SelectTrigger>
                    <SelectContent>
                    {adTypes.map((type) => (
                        <SelectItem key={type} value={type}>
                          {type}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              )}
            />

            <Controller
              name="preco"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="preco">Preço</Label>
                  <Input
                    {...field}
                    id="preco"
                    type="number"
                    placeholder="Preço (se aplicável)"
                    disabled={adType === "BUSCA"}
                  />
                  {errors.preco && <p className="text-red-500">{errors.preco.message}</p>}
                </div>
              )}
            />

            <Controller
              name="contato"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="contact">Informações de Contato</Label>
                  <Input {...field} id="contact" placeholder="Seu email ou telefone" />
                  {errors.contato && <p className="text-red-500">{errors.contato.message}</p>}
                </div>
              )}
            />

            <Controller
              name="bairro"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="bairro">Bairro</Label>
                  <Input {...field} id="contact" placeholder="Seu email ou telefone" />
                  {errors.contato && <p className="text-red-500">{errors.contato.message}</p>}
                </div>
              )}
            />

            <Controller
              name="cidade"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="cidade">Cidade</Label>
                  <Select onValueChange={field.onChange} defaultValue={field.value}>
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
                  {errors.cidade && <p className="text-red-500">{errors.cidade.message}</p>}
                </div>
              )}
            />

            <Controller
              name="categoria"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <Label htmlFor="categoria">Categoria</Label>
                  <Select onValueChange={field.onChange} defaultValue={field.value}>
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
                  {errors.categoria && <p className="text-red-500">{errors.categoria.message}</p>}
                </div>
              )}
            />

            <Button type="submit" className="w-full">
              Criar Anúncio
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
