"use client"
import { redirect, useParams, usePathname, useRouter } from 'next/navigation'
import React, { useState, useRef } from 'react'
import { useSession } from 'next-auth/react'
import BranchCreateForm from '@/app/components/forms/BranchForm'
import BranchForm from '@/app/components/forms/BranchForm'
import CancelButton from '@/app/components/buttons/CancelButton'
import { OuterContainer } from '@/app/components/layout/PageContainers'

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
    <OuterContainer className='max-w-xl mx-auto p-6 mt-8 font-inter'>
      <h1 className='text-3xl font-bold mb-6 text-center'>Create Branch</h1>
      <form className='flex flex-col gap-6' onSubmit={handleSubmit}>
        <ul>
          {forms.map((value) => (
            <li key={value.props.index} className='flex flex-col gap-4'>
              {value}
              <button className='bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition duration-300 ease-in-out shadow-md font-semibold self-end mt-2'
              onClick={() => {
                setForms(forms.filter((val) => {
                  if (val.props.index === value.props.index) {
                    sendData.current = sendData.current.filter((v) => v.index !== val.props.index)
                    return false
                  }
                  return true
                }))
              }}
              >Remove branch</button>
            </li>
          ))}
        </ul>
        <button
          className='bg-green-600 text-white px-5 py-2 rounded-lg hover:bg-green-700 transition duration-300 ease-in-out shadow-md font-semibold mt-4'
          onClick={() => { setForms([...forms, <BranchForm index={counter} getData={handleData} />]); setCounter(counter + 1) }}>Add branch</button>
        <CancelButton/>
        <button
          className='bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold mt-4'
          type='submit'>Create</button>
      </form>
    </OuterContainer>
  )
}

export default createBranch