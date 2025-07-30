'use client'

import { useSession } from 'next-auth/react'
import { useParams } from 'next/navigation'
import React, { useEffect, useState } from 'react'

interface Response {
  items: Item[],
  totalCount: number
}

const OfferDetails = () => {
  const [apiData, setApiData] = useState<Response | null>();
  const { data: session, status } = useSession()
  const params = useParams()
  const id = params.id

  useEffect(() => {
    if(!session){
      fetch(`http://localhost:8080/api/GuestQueries/offers/${id}`)
      .then(res => res.json())
      .then(setApiData)
    }
    else{
      fetch(`http://localhost:8080/api/CompanyUser/offers/${id}`, {
        headers: {
          Authorization: `Bearer ${session.user.token}`,
        }
      })
      .then(res => res.json())
      .then(setApiData)
    }
  }, [session])

  return (
    <div>
      <h1>OfferDetails </h1>
      <>{apiData && apiData.totalCount}</>
    </div>
  )
}

export default OfferDetails