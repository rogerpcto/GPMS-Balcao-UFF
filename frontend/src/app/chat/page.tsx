import Image from "next/image";

interface ChatItem {
  id: string;
  contactName: string;
  productName: string;
  productImageUrl: string;
}

const chatData: ChatItem[] = [
  {
    id: "1",
    contactName: "João Silva",
    productName: "Carro usado Toyota Corolla",
    productImageUrl: "/imgs/produto.png",
  },
  {
    id: "2",
    contactName: "Maria Santos",
    productName: "Apartamento 2 quartos - Centro",
    productImageUrl: "/imgs/produto.png",
  },
  {
    id: "3",
    contactName: "Carlos Oliveira",
    productName: "iPhone 12 Pro - 128GB",
    productImageUrl: "/imgs/produto.png",
  },
  {
    id: "4",
    contactName: "Ana Rodrigues",
    productName: "Bicicleta Mountain Bike",
    productImageUrl: "/imgs/produto.png",
  },
  {
    id: "5",
    contactName: "Pedro Almeida",
    productName: "Sofá de couro 3 lugares",
    productImageUrl: "/imgs/produto.png",
  },
  // Adicione mais itens conforme necessário
];

export default function ChatList() {
  return (
    <>
      <h2 className="text-2xl font-bold p-6 border-b">Conversas</h2>
      {chatData.map((chat) => (
        <div
          key={chat.id}
          className="flex items-center p-6 border-b hover:bg-muted/50 cursor-pointer"
        >
          <div className="flex-shrink-0 mr-4">
            <Image
              src={chat.productImageUrl}
              alt={chat.productName}
              width={80}
              height={80}
              className="rounded-md object-cover"
            />
          </div>
          <div className="flex-grow">
            <h3 className="font-semibold text-lg text-foreground">
              {chat.contactName}
            </h3>
            <p className="text-sm text-muted-foreground">{chat.productName}</p>
          </div>
        </div>
      ))}
    </>
  );
}
