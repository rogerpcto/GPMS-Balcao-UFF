"use client"

import { useState } from "react"
import { Info, Trash2 } from 'lucide-react'
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"

import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Switch } from "@/components/ui/switch"
import { 
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { StatusBadge, StatusType } from "@/app/meus-anuncios/page"
import { Assunto } from "@/app/perfil/[id]/page"
import { editAd, updateAdStatus } from "@/data-access/advertisement"
import { Label } from "./ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "./ui/select"

const formSchema = z.object({
  titulo: z.string().min(3, "Título deve ter no mínimo 3 caracteres"),
  preco: z.string().regex(/^\d+(\.\d{1,2})?$/, "Preço inválido"),
  descricao: z.string().min(10, "Descrição deve ter no mínimo 10 caracteres"),
  quantidade: z.string().min(1, "Quantidade deve ser pelo menos 1")
})

interface AdDialogProps {
  ad: Assunto;
  isOwner: boolean;
  status: string
  currentUserId: string
  onToggleActive?: (id: string, isActive: boolean) => Promise<void>
  purchaseId: number
}

export function AdDialog({ 
  ad, 
  isOwner,
  status,
  currentUserId, 
  onToggleActive,
  purchaseId 
}: AdDialogProps) {
  const [isOpen, setIsOpen] = useState(false)
  const [active, setActive] = useState(ad.ativo)
  const [isLoading, setIsLoading] = useState(false)

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      titulo: ad.titulo,
      preco: ad.preco.toString(),
      descricao: ad.descricao,
      quantidade: String(ad.quantidade),
    },
  })

  async function onSubmit(values: z.infer<typeof formSchema>) {

    editAd(String(ad.id), values).then(res => {
        if(!res) return;
        setIsOpen(false);
    })
  }

  async function handleToggleActive(checked: boolean) {
    try {
      setIsLoading(true)
      await onToggleActive?.(String(ad.id), checked)
    } catch (error) {
      console.error(error)
    } finally {
      setIsLoading(false)
    }
  }

  const statusLabels: Record<StatusType, string> = {
    AGUARDANDO_PAGAMENTO: "Aguardando Pagamento",
    PRODUTO_RECEBIDO: "Produto Recebido",
    PAGAMENTO_EFETUADO: "Pagamento Efetuado",
    CONCLUIDO: "Concluído",
    PAGAMENTO_CONFIRMADO: "Pagamento Confirmado",
    NEGOCIANDO: "Negociando",
    COMPRADOR_AVALIADO: "Comprador Avaliado",
    VENDEDOR_AVALIADO: "Vendedor Avaliado"
  }

  const handleStatusChange = (newStatus: StatusType) => {

    updateAdStatus(ad.id, purchaseId, newStatus).then(res => {
      console.log("🚀 ~ updateAdStatus ~ res:", res)
    }) 
  }

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" size="sm">
          <Info className="w-4 h-4 mr-2" />
          Detalhes
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        {isOwner ? (
          <>
            <DialogHeader>
              <DialogTitle>Editar Anúncio</DialogTitle>
            </DialogHeader>
            <Form {...form}>
              <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
                <FormField
                  control={form.control}
                  name="titulo"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Título</FormLabel>
                      <FormControl>
                        <Input {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="preco"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Preço</FormLabel>
                      <FormControl>
                        <Input {...field} type="number" step="0.01" />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="descricao"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Descrição</FormLabel>
                      <FormControl>
                        <Textarea {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="quantidade"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Quantidade</FormLabel>
                      <FormControl>
                        <Input {...field} type="number" min="1" />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <div className="grid gap-2">
                  <Label htmlFor="status" className="text-base font-medium">
                    Status
                  </Label>
                  <Select onValueChange={(value) => handleStatusChange(value as StatusType)}>
                    <SelectTrigger id="status">
                      <SelectValue placeholder="Selecione um novo status" />
                    </SelectTrigger>
                    <SelectContent>
                      {Object.entries(statusLabels).map(([value, label]) => (
                        <SelectItem key={value} value={value}>
                          {label}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>

                <div className="flex items-center space-x-2">
                  <Switch
                    checked={active}
                    onCheckedChange={handleToggleActive}
                    disabled={isLoading}
                  />
                  <span>Anúncio {active ? "ativo" : "inativo"}</span>
                </div>

                <DialogFooter className="gap-2">
                  <Button type="submit" disabled={isLoading}>
                    Salvar alterações
                  </Button>
                </DialogFooter>
              </form>
            </Form>
          </>
        ) : (
          <>
            <DialogHeader>
              <DialogTitle>{ad.titulo}</DialogTitle>
              <DialogDescription className="space-y-2">
                <p>Preço: R$ {ad.preco.toFixed(2)}</p>
                <p>Status: <StatusBadge status={status as any} /></p>
                <p className="whitespace-pre-wrap">{ad.descricao}</p>
              </DialogDescription>
            </DialogHeader>
          </>
        )}
      </DialogContent>
    </Dialog>
  )
}

