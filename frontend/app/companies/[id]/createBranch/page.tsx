"use client"
import {useParams, useRouter } from 'next/navigation'
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

const CreateBranch = () => {
  const router = useRouter()
  const { data: session } = useSession({
    required: true,
    onUnauthenticated() {
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
  const [forms, setForms] = useState([<BranchCreateForm key={0} index={0} getData={handleData} />]);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()

    const sendArray: SendData[] = []
    sendData.current.sort((value) => value.index).map((value) => {
      const { ...rest } = value
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
      router.replace(`/companies/${id}`)
    }
    else {
      alert("Failed to create branch(es)")
    }
  }

  return (
    <OuterContainer className='max-w-xl mx-auto'>
          <h1 className='text-4xl font-extrabold mb-8 text-center text-gray-900 dark:text-gray-100'>Create New Branch(es)</h1>
          <form className='flex flex-col gap-8' onSubmit={handleSubmit}>
            <div className='space-y-6'>
              {forms.length === 0 && (
                <p className="text-center text-gray-600 dark:text-gray-400 text-lg">
                  No branches added yet. Click +` to add branch.
                </p>
              )}
              {forms.map((value) => (

                <li key={value.props.index} className='list-none bg-gray-50 dark:bg-gray-700 p-6 rounded-lg shadow-lg border border-gray-200 dark:border-gray-600 relative'>
                  <h2 className='text-2xl font-semibold mb-6 text-gray-800 dark:text-gray-200'>Branch #{value.props.index + 1}</h2>
                  {value}
                  {forms.length > 1 && (
                    <button
                      type="button"
                      className='absolute top-4 right-4 bg-red-600 text-white p-2 rounded-full hover:bg-red-700 transition duration-300 ease-in-out shadow-md font-bold text-lg w-8 h-8 flex items-center justify-center'
                      title="Remove this branch"
                      onClick={() => {
                        setForms(forms.filter((val) => {
                          if (val.props.index === value.props.index) {
                            sendData.current = sendData.current.filter((v) => v.index !== val.props.index)
                            return false
                          }
                          return true
                        }))
                      }}
                    >
                      {"X"}
                    </button>
                  )}
                </li>
              ))}
            </div>

            <div className="flex justify-end mb-4">
              <button
                type="button"
                className='bg-green-600 text-white p-3 rounded-lg hover:bg-green-700 transition duration-300 ease-in-out shadow-md font-semibold flex items-center justify-center'
                onClick={() => { setForms([...forms, <BranchForm key={counter} index={counter} getData={handleData} />]); setCounter(counter + 1) }}
                title="Add new branch"> Next branch {/* or just rounded-full and  '+' */ }
              </button>
            </div>
            <button
              className='bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold mt-4'
              type='submit'> Create all
            </button>
            <CancelButton/>
          </form>
    </OuterContainer>
  )
}

export default CreateBranch