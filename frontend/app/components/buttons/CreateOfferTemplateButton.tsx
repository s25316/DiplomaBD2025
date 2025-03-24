"use client"
import React from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const CreateOfferTemplateButton = () => {
  return (
    <Link href={`${usePathname()}/createOfferTemplate`} className="text-blue-600">Create offer template</Link>
  )
}

export default CreateOfferTemplateButton