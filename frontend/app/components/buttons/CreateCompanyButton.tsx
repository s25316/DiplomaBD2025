"use client"
import Link from 'next/link'
import React from 'react'

const CreateCompany = () => {
    return (
        <Link href="/profile/createCompany" className="text-blue-600">Create company</Link>
    )
}

export default CreateCompany