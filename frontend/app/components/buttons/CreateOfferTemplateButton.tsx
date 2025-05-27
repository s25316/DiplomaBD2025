"use client"
import React from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const CreateOfferTemplateButton = () => {
  return (
    <Link href={`${usePathname()}/createOfferTemplate`} className="inline-block mt-2 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">Create new offer template</Link>
  )
}

export default CreateOfferTemplateButton