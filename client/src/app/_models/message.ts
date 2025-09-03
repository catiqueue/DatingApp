export type Message = {
  id: number
  senderId: number
  senderUsername: string
  senderAvatarUrl: string
  recipientId: number
  recipientUsername: string
  recipientAvatarUrl: string
  content: string
  readAt?: Date
  sentAt: Date
}
