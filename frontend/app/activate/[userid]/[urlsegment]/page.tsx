"use client"

import React from 'react'
import { usePathname, useRouter } from 'next/navigation'

const AccountActivation = () => {
    const pathname = usePathname()
    const pathnames = pathname.split("/")
    
    const router = useRouter()

    const fetchDummy = async () => {
        const res = await fetch(`http://localhost:8080/api/User/activation/${pathnames.at(2)}/${pathnames.at(3)}`, {
            method: "POST",
            headers:{
                "Content-Type": "application/json",
                "accept": "*/*",
            },
        })

        if (res.ok){        
            router.push("/api/auth/signin")
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