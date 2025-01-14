"use client";

import { useEffect, useState } from "react";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { getProfile } from "@/data-access/user";
import { getPurchases } from "@/data-access/purchase";
import { formatDate } from "@/app/page";


type Profile = {
  id: number;
  nome: string;
  email: string;
  nota: number;
  perfil: string;
};


type History = Transaction[];
export interface Transaction {
  id: number
  quantidade: number
  nota: number
  status: string
  comprador: Comprador
  assunto: Assunto
  mensagens: Mensagen[]
}

export interface Comprador {
  id: number
  nome: string
  email: string
  nota: number
  perfil: string
}

export interface Assunto {
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

export interface Mensagen {
  id: number
  timeStamp: string
  conteudo: string
  proprietario: boolean
}


export default function ProfilePage({ params }: {
  params: { id: string}
}) {

  const [profile, setProfile] = useState<Profile | null>(null);
  const [transactionHistory, setTransactionHistory] = useState<History | null>(null);

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;
  const totalPages = transactionHistory ? Math.ceil(transactionHistory.length / itemsPerPage) : 0;

  const paginatedTransactions = transactionHistory && transactionHistory.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );


  useEffect(() => {

    const fetchProfile = async () => {
      getProfile(params.id).then(res => {
        setProfile(res);
      });
    };
  
    const fetchHistory = async () => {
      getPurchases(params.id).then(res => {
        setTransactionHistory(res)
      });
    }

    fetchProfile();
    fetchHistory();
  }, []);

  return (
    <div className="container mx-auto py-8">
      <Card className="mb-8">
        <CardHeader>
          <CardTitle>Perfil do Usuário</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-2">
            <p>
              <strong>Nome:</strong> {profile?.nome}
            </p>
            <p>
              <strong>E-mail:</strong> {profile?.email}
            </p>
            <p>
              <strong>Reputação:</strong> {profile?.nota} / 5
            </p>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Histórico de Transações</CardTitle>
        </CardHeader>
        <CardContent>
          <Table>
            <TableCaption>Histórico de compras e vendas</TableCaption>
            <TableHeader>
              <TableRow>
                <TableHead>Tipo</TableHead>
                <TableHead>Item</TableHead>
                <TableHead>Data</TableHead>
                <TableHead>Valor</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {paginatedTransactions?.map((transaction) => (
                <TableRow key={transaction.assunto.id}>
                  <TableCell>{transaction.assunto.quantidade}</TableCell>
                  <TableCell>{transaction.assunto.titulo}</TableCell>
                  <TableCell>{formatDate(transaction.assunto.dataCriacao)}</TableCell>
                  <TableCell>{transaction.assunto.preco}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
          <div className="flex justify-between items-center mt-4">
            <Button
              onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
              disabled={currentPage === 1}
            >
              Anterior
            </Button>
            <span>
              Página {currentPage} de {totalPages}
            </span>
            <Button
              onClick={() =>
                setCurrentPage((prev) => Math.min(prev + 1, totalPages))
              }
              disabled={currentPage === totalPages}
            >
              Próxima
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}