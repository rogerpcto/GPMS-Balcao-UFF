"use client";

import { useEffect, useState } from "react";
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
import { useAuth } from "@/context/auth-context";
import { getPurchases, sendRating } from "@/data-access/purchase";
import { Assunto as Ad, Transaction } from "../perfil/[id]/page";

import { cn } from "@/lib/utils"
import { AdDialog } from "@/components/ad-dialog";
import { disativateAd, getMyAds, sendMessage } from "@/data-access/advertisement";
import { ChatModal } from "@/components/chat-modal";

export type StatusType = 
  | "AGUARDANDO_PAGAMENTO"
  | "PRODUTO_RECEBIDO"
  | "PAGAMENTO_EFETUADO"
  | "CONCLUIDO"
  | "PAGAMENTO_CONFIRMADO"
  | "NEGOCIANDO"
  | "COMPRADOR_AVALIADO"
  | "VENDEDOR_AVALIADO"

const statusMap: Record<StatusType, string> = {
  AGUARDANDO_PAGAMENTO: "Aguardando pagamento",
  PRODUTO_RECEBIDO: "Produto recebido",
  PAGAMENTO_EFETUADO: "Pagamento efetuado",
  CONCLUIDO: "Concluído",
  PAGAMENTO_CONFIRMADO: "Pagamento confirmado",
  NEGOCIANDO: "Negociando",
  COMPRADOR_AVALIADO: "Comprador avaliado",
  VENDEDOR_AVALIADO: "Vendedor avaliado"
}

const statusColorMap: Record<StatusType, string> = {
  AGUARDANDO_PAGAMENTO: "bg-yellow-500 hover:bg-yellow-600",
  PRODUTO_RECEBIDO: "bg-green-500 hover:bg-green-600",
  PAGAMENTO_EFETUADO: "bg-blue-500 hover:bg-blue-600",
  CONCLUIDO: "bg-green-700 hover:bg-green-800",
  PAGAMENTO_CONFIRMADO: "bg-blue-700 hover:bg-blue-800",
  NEGOCIANDO: "bg-purple-500 hover:bg-purple-600",
  COMPRADOR_AVALIADO: "bg-gray-500 hover:bg-gray-600",
  VENDEDOR_AVALIADO: "bg-gray-500 hover:bg-gray-600"
}

interface StatusBadgeProps {
  status: StatusType
  className?: string
}

export function StatusBadge({ status, className }: StatusBadgeProps) {
  return (
    <Badge 
      className={cn(
        "text-white font-medium",
        statusColorMap[status],
        className
      )}
    >
      {statusMap[status]}
    </Badge>
  )
}

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

const AdCard = ({ buyerName, purchaseId, ad, status, showChat, userId, role, messages }: { buyerName: string, purchaseId: number, ad: Ad, status: StatusType, showChat: boolean, role: "VENDEDOR" | "COMPRADOR", userId: string | undefined, messages: any }) => {
  const [isRatingDialogOpen, setIsRatingDialogOpen] = useState(false);
  const [openChat, setOpenChat] = useState(false);
  const [rating, setRating] = useState(0);

  const handleRatingSubmit = () => {
    
    if(!userId) return; 

    sendRating(userId, ad.id, rating, role).then(res => {
      if(!res) return;
      
      setIsRatingDialogOpen(false);
      setRating(0);
    })
  };

  const handleToggleActive = async (id: string, isActive: boolean) => {
    if(!isActive) {
      disativateAd(id, ad).then(res => {
        if(!res) return;
      })
    }
  }

  function sendMessages(newMessage: string) {
    sendMessage(purchaseId, ad.id, newMessage).then(res => {
      console.log("🚀 ~ sendMessage ~ res:", res);      
    });
  }
  
  return (
    <>
    <Card>
      {ad.id}
      <CardHeader>
        <CardTitle className="flex items-center justify-between">
          {ad.titulo}
          {status && <StatusBadge status={status} />}
        </CardTitle>
        {/* <CardDescription>
          {ad.category} - {ad.type === "oferta" ? "Oferta" : "Busca"}
        </CardDescription> */}
      </CardHeader>
      <CardContent className="h-12">
        {ad.preco && (
          <p className="text-2xl font-bold">R$ {ad.preco.toFixed(2)}</p>
        )}
      </CardContent>
      <CardFooter className="flex justify-between mt-auto">
        <AdDialog 
          ad={ad}
          isOwner={role === 'VENDEDOR'}
          status={status}
          purchaseId={purchaseId}
          currentUserId={userId as string}
          onToggleActive={handleToggleActive}
        />

        {showChat && (
          <Button onClick={() => setOpenChat(true)} variant="secondary" size="sm">
            <MessageCircle className="w-4 h-4 mr-2" />
            Chat
          </Button>
        )}

        {status === "CONCLUIDO" && (
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

    <ChatModal 
      isOpen={openChat} 
      messages={messages} 
      buyerName={buyerName}
      sellerName={ad.proprietario.nome} 
      onClose={() => { setOpenChat(false) }} 
      onSendMessage={(newMessage) => sendMessages(newMessage)} 
    />
      
    </>
  );
};


export default function MeusAnuncios() {

  const [ myAds, setMyAds] = useState<Transaction[]>([]);
  const [ participatedAds, setParticipatedAds] = useState<Transaction[]>([]);

  const { user } = useAuth();
  const userId = user?.nameid;

  useEffect(() => {

    const fetchMyPurchases = async () => {
      if(!userId) return;

      getPurchases(userId).then(res => {
        setParticipatedAds(res)
      });
    }

    const fetchMyAds = async () => {
      if(!userId) return;

      getMyAds(userId).then(res => {
        setMyAds(res);
      });
    }

    fetchMyPurchases();
    fetchMyAds();
  }, []);

  return (
    <div className="container mx-auto py-8">
      <h1 className="text-3xl font-bold mb-6">Meus Anúncios</h1>
      <Tabs defaultValue="my-ads">
        <TabsList className="grid w-full grid-cols-2">
          <TabsTrigger disabled={myAds.length === 0} value="my-ads">Meus Anúncios</TabsTrigger>
          <TabsTrigger disabled={participatedAds.length === 0} value="participated">Anúncios Participados</TabsTrigger>
        </TabsList>
        <TabsContent value="my-ads">
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {myAds.map((ad) => (
              <AdCard 
                key={ad.id} 
                ad={ad} 
                role="VENDEDOR"
                purchaseId={ad.id} 
                userId={userId} 
                status={ad.status as StatusType} 
                showChat={ad.status !== "CONCLUIDO" && !myAds.includes(ad)}
              />
            
            ))}
          </div>
        </TabsContent>
        <TabsContent value="participated">
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {participatedAds.map((ad) => (
              	<AdCard 
                   key={ad.id} 
                   userId={userId} 
                   purchaseId={ad.id} 
                   buyerName={ad.comprador.nome}
                   role={ad.assunto.proprietario.id === Number(userId) ? 'VENDEDOR' : 'COMPRADOR'}
                   ad={ad.assunto}
                   messages={ad?.mensagens}
                   status={ad.status as StatusType}
                   showChat={ad.status !== "CONCLUIDO"} 
                />
            	))}
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
