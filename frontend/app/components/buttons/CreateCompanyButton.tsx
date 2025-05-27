"use client"
import Link from 'next/link'
import React from 'react'

const CreateCompany = () => {
    return (
        <Link href="/profile/createCompany" className="inline-block mt-6 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">Create company</Link>
    )
}

export default CreateCompany