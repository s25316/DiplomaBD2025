"use client"
import { redirect, useParams, usePathname, useRouter } from 'next/navigation'
import React, { useState, useRef } from 'react'
import { useSession } from 'next-auth/react'
import BranchCreateForm from '@/app/components/forms/BranchForm'
import BranchForm from '@/app/components/forms/BranchForm'

interface SendData {
  name: string,
  description: string | null,
  address: {
    countryName: string,
    stateName: string,
    cityName: string,
    streetName: string | null,
    houseNumber: string,
    apartmentNumber: string | null,
    postCode: string,
    lon: number,
    lat: number,
  },
}

interface Data extends SendData {
  index: number,
}

const createBranch = () => {
  const { data: session } = useSession({
    required: true,
    onUnauthenticated() {
      const router = useRouter()
      router.back()
    },
  });

  //check if the owner of the company

  const sendData = useRef<Data[]>([])

  const handleData = (data: Data) => {
    sendData.current = sendData.current.filter((value) => value.index !== data.index).concat(data)
  }

  const { id } = useParams();
  const [counter, setCounter] = useState(1);
  const [forms, setForms] = useState([<BranchCreateForm index={0} getData={handleData} />]);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()

    var sendArray: SendData[] = []
    sendData.current.sort((value) => value.index).map((value) => {
      const { index, ...rest } = value
      sendArray.push(rest)
    })

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/branches`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user.token}`
      },
      body: JSON.stringify(sendArray)
    })

    if (res.ok) {
      alert("Branch(es) created")
      redirect(`/companies/${id}`)
    }
    else {
      alert("Failed to create branch(es)")
    }
  }

  return (
    <div className="max-w-xl mx-auto">
      <h1>Create Branch</h1>
      <form className='flex flex-col gap-4' onSubmit={handleSubmit}>
        <ul>
          {forms.map((value) => (
            <li key={value.props.index} className='flex flex-col gap-4'>
              {value}
              <button onClick={() => {
                setForms(forms.filter((val) => {
                  if (val.props.index === value.props.index) {
                    sendData.current = sendData.current.filter((v) => v.index !== val.props.index)
                    return false
                  }
                  return true
                }))
              }}>Remove branch</button>
            </li>
          ))}
        </ul>
        <button onClick={() => { setForms([...forms, <BranchForm index={counter} getData={handleData} />]); setCounter(counter + 1) }}>Add branch</button>
        <button type='submit'>Create</button>
      </form>
    </div>
  )
}

export default createBranch