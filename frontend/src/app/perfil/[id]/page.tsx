"use client";

import { useState } from "react";
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

// Simulated user data
const userData = {
  name: "João Silva",
  email: "joao.silva@id.uff.br",
  reputation: 4.5,
};

// Simulated transaction history
const transactionHistory = [
  {
    id: 1,
    type: "Compra",
    item: "Smartphone",
    date: "2023-07-01",
    value: "R$ 1000,00",
  },
  {
    id: 2,
    type: "Venda",
    item: "Laptop",
    date: "2023-07-15",
    value: "R$ 2500,00",
  },
  {
    id: 3,
    type: "Compra",
    item: "Headphones",
    date: "2023-08-02",
    value: "R$ 300,00",
  },
  // Add more transactions as needed
];

export default function ProfilePage() {
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;
  const totalPages = Math.ceil(transactionHistory.length / itemsPerPage);

  const paginatedTransactions = transactionHistory.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  return (
    <div className="container mx-auto py-8">
      <Card className="mb-8">
        <CardHeader>
          <CardTitle>Perfil do Usuário</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-2">
            <p>
              <strong>Nome:</strong> {userData.name}
            </p>
            <p>
              <strong>E-mail:</strong> {userData.email}
            </p>
            <p>
              <strong>Reputação:</strong> {userData.reputation} / 5
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
              {paginatedTransactions.map((transaction) => (
                <TableRow key={transaction.id}>
                  <TableCell>{transaction.type}</TableCell>
                  <TableCell>{transaction.item}</TableCell>
                  <TableCell>{transaction.date}</TableCell>
                  <TableCell>{transaction.value}</TableCell>
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