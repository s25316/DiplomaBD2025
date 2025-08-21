"use client"

import React from 'react'
import { usePathname, useRouter } from 'next/navigation'

const AccountActivation = () => {
    const pathname = usePathname()
    const pathnames = pathname.split("/")
    
    const router = useRouter()

    const backUrl = process.env.NEXT_PUBLIC_API_URL

    const fetchDummy = async () => {
        const res = await fetch(`${backUrl}/api/User/activation/${pathnames.at(2)}/${pathnames.at(3)}`, {
            method: "POST",
            headers:{
                "Content-Type": "application/json",
                "accept": "*/*",
            },
        })

        if (res.ok){        
            router.push("/login")
        }
        else{
            alert("There is problem activating your account")
        }
    }

    fetchDummy()

  return (
    <div>Processing...</div>
  )
}

export default AccountActivation