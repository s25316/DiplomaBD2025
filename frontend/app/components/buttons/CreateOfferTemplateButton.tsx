"use client"
import React from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const CreateOfferTemplateButton = () => {
  return (
    <Link href={`${usePathname()}/createOfferTemplate`}>Create offer template</Link>
  )
}

export default CreateOfferTemplateButton