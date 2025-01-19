"use client"

import { useState } from 'react'
import { X, Send } from 'lucide-react'
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { ScrollArea } from "@/components/ui/scroll-area"
import { format } from 'date-fns'

interface Message {
  id: number
  timeStamp: string
  conteudo: string
  proprietario: boolean
}

interface ChatModalProps {
  isOpen: boolean
  onClose: () => void
  messages: Message[]
  buyerName: string
  sellerName: string
  onSendMessage: (message: string) => void
}

export function ChatModal({ isOpen, onClose, messages, buyerName, sellerName, onSendMessage }: ChatModalProps) {
  const [newMessage, setNewMessage] = useState('')

  if (!isOpen) return null

  const handleSendMessage = () => {
    if (newMessage.trim()) {
      onSendMessage(newMessage)
      setNewMessage('')
    }
  }

  return (
    <div className="fixed inset-0 bg-background z-50 flex flex-col">
      <div className="flex justify-between items-center p-4 border-b">
        <h2 className="text-lg font-semibold">Chat</h2>
        <Button variant="ghost" size="icon" onClick={onClose}>
          <X className="h-6 w-6" />
        </Button>
      </div>
      <ScrollArea className="flex-grow p-4">
        {messages.map((message) => (
          <div
            key={message.id}
            className={`mb-4 ${
              message.proprietario ? 'text-right' : 'text-left'
            }`}
          >
            <div
              className={`inline-block p-3 rounded-lg ${
                message.proprietario
                  ? 'bg-primary text-primary-foreground'
                  : 'bg-muted'
              }`}
            >
              <p className="text-sm font-semibold mb-1">
                {message.proprietario ? sellerName : buyerName}
              </p>
              <p>{message.conteudo}</p>
              <p className="text-xs mt-1 opacity-70">
                {format(new Date(message.timeStamp), 'dd/MM/yyyy HH:mm')}
              </p>
            </div>
          </div>
        ))}
      </ScrollArea>
      <div className="p-4 border-t flex">
        <Input
          value={newMessage}
          onChange={(e) => setNewMessage(e.target.value)}
          placeholder="Digite sua mensagem..."
          className="flex-grow mr-2"
          onKeyPress={(e) => {
            if (e.key === 'Enter') {
              handleSendMessage()
            }
          }}
        />
        <Button onClick={handleSendMessage}>
          <Send className="h-4 w-4 mr-2" />
          Enviar
        </Button>
      </div>
    </div>
  )
}