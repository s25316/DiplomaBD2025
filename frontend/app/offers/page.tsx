'use client'

import React, { useEffect, useState } from 'react'
import Link from 'next/link'
import { useSession } from 'next-auth/react';
import { OfferTemplate } from '@/types/offerTemplate';

interface Response {
  items: Item[],
  totalCount: number
}

interface Item {
  offer: Offer,
  offerTemplate: OfferTemplate,
  company: Company,
  branch: Branch,
  contractConditions: ContractConditions[] | null,
}

interface Filter {
  searchText: string,
  status: number,
  ascending: boolean,
  orderBy: number | null,
  regon: string,
  nip: string,
  krs: string,
  hasValue: boolean | null,
  isNegotiable: boolean | null,
  isPaid: boolean | null,
  salaryPerHourMin: number | null,
  salaryPerHourMax: number | null,
  salaryMin: number | null,
  salaryMax: number | null,
  hoursMin: number | null,
  hoursMax: number | null,
  lon: number | null,
  lat: number | null,
  publicationStartFrom: Date | null,
  publicationStartTo: Date | null,
  publicationEndFrom: Date | null,
  publicationEndTo: Date | null,
  employmentLengthFrom: number | null,
  employmentLengthTo: number | null,
  page: number,
  itemsPerPage: number,
  skillsIds: number[],
  contractParameterIds: number[]
}

interface OrderBy {
  id: number,
  name: string
}

interface Status {
  id: number,
  name: string
}

interface ContractParameter {
  contractParameterId: number,
  name: string
}

interface Skill {
  skillId: number,
  name: string
}

const Offers = () => {
  const [apiData, setApiData] = useState<Response | null>();
  const [filters, setFilters] = useState<Filter>({
    searchText: '',
    status: 2,
    ascending: false,
    orderBy: null,
    regon: '',
    nip: '',
    krs: '',
    hasValue: null,
    isNegotiable: null,
    isPaid: null,
    salaryPerHourMin: null,
    salaryPerHourMax: null,
    salaryMin: null,
    salaryMax: null,
    hoursMin: null,
    hoursMax: null,
    lon: null,
    lat: null,
    publicationStartFrom: null,
    publicationStartTo: null,
    publicationEndFrom: null,
    publicationEndTo: null,
    employmentLengthFrom: null,
    employmentLengthTo: null,
    page: 1,
    itemsPerPage: 10,
    skillsIds: [],
    contractParameterIds: [],
  });
  const [orderByDictinary, setOrderByDictinary] = useState<OrderBy[]>([]);
  const [statusDictinary, setStatusDictinary] = useState<Status[]>([]);
  const [contractParametersDictinary, setContractParametersDictinary] = useState<ContractParameter[]>([]);
  const [skillDictinary, setSkillDictinary] = useState<Skill[]>([]);

  const { data: session } = useSession();

  useEffect(() => {
    const filterQuery =
  `?search=${filters.searchText}` +
  `&status=${filters.status}` +
  `&ascending=${filters.ascending}` +
  `&orderBy=${filters.orderBy || ""}` +
  `&regon=${filters.regon}` +
  `&nip=${filters.nip}` +
  `&krs=${filters.krs}` +
  `&isNegotiable=${filters.isNegotiable || ""}` +
  `&isPaid=${filters.isPaid || ""}` +
  `&salaryPerHourMin=${filters.salaryPerHourMin || ""}` +
  `&salaryPerHourMax=${filters.salaryPerHourMax || ""}` +
  `&salaryMin=${filters.salaryMin || ""}` +
  `&salaryMax=${filters.salaryMax || ""}` +
  `&hoursMin=${filters.hoursMin || ""}` +
  `&hoursMax=${filters.hoursMax || ""}` +
  `&lon=${filters.lon || ""}` +
  `&lat=${filters.lat || ""}` +
  `&publicationStartFrom=${filters.publicationStartFrom || ""}` +
  `&publicationStartTo=${filters.publicationStartTo || ""}` +
  `&publicationEndFrom=${filters.publicationEndFrom || ""}` +
  `&publicationEndTo=${filters.publicationEndTo || ""}` +
  `&employmentLengthFrom=${filters.employmentLengthFrom || ""}` +
  `&employmentLengthTo=${filters.employmentLengthTo || ""}` +
  `&page=${filters.page}` +
  `&itemsPerPage=${filters.itemsPerPage}`;

    fetch('http://localhost:8080/api/GuestQueries/offers' + filterQuery, {
      headers: {
        contractParameterIds: filters.contractParameterIds.map(x => `${x}`).join(","),
        skillsIds: filters.skillsIds.map(x => `${x}`).join(",")
      }
    })
      .then(res => res.json())
      .then(setApiData)
  }, [session, filters]);

  useEffect(() => {
    fetch('http://localhost:8080/api/QueryParameters/offers/orderBy')
      .then(res => res.json())
      .then(setOrderByDictinary)
    fetch('http://localhost:8080/api/QueryParameters/offer/statuses')
      .then(res => res.json())
      .then(setStatusDictinary)
    fetch('http://localhost:8080/api/Dictionaries/contractParameters')
      .then(res => res.json())
      .then(setContractParametersDictinary)
    fetch('http://localhost:8080/api/Dictionaries/skills')
      .then(res => res.json())
      .then(setSkillDictinary)
  }, [])

  return (
    <div className='flex m-0 p-6 bg-white dark:bg-gray-900 rounded-lg shadow-xl mt-8 font-inter text-gray-900 dark:text-gray-100'>
      <div className='w-[330px] h-screen p-[20px] pl-[5px] overflow-y-auto sticky top-0'>
        <label>Search:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='text' onChange={x => setFilters({ ...filters, searchText: x.target.value })} /><br />
        <label>Status:</label><br />
        <select className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' onChange={x => setFilters({ ...filters, status: Number(x.target.value) })}>
          {statusDictinary && statusDictinary.map(x => (
            <option key={x.id} value={x.id} selected={x.id === filters.status}>{x.name}</option>
          ))}
        </select><br />
        <label>Sort:</label><br />
        <select className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' onChange={x => setFilters({ ...filters, ascending: x.target.value === "1" ? true : false })}>
          <option key={1} value={1} selected={filters.ascending === true}>Ascending</option>
          <option key={0} value={0} selected={filters.ascending === false}>Descending</option>
        </select><br />
        <label>Order by:</label><br />
        <select className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' onChange={x => setFilters({ ...filters, orderBy: x.target.value === "null" ? null : Number(x.target.value) })}>
          <option key={null} value={"null"} selected={filters.orderBy === null}>-----</option>
          {orderByDictinary && orderByDictinary.map(x => (
            <option key={x.id} value={x.id} selected={filters.orderBy === x.id}>{x.name}</option>
          ))}
        </select><br />
        <label>Regon:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, regon: x.target.value })} /><br />
        <label>Nip:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, nip: x.target.value })} /><br />
        <label>Krs:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, krs: x.target.value })} /><br />
        <label>Negotiatable:</label><br />
        <select className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' onChange={x => setFilters({ ...filters, isNegotiable: x.target.value === "null" ? null : x.target.value === "1" ? true : false })}>
          <option key={null} value={"null"} selected={filters.isNegotiable === null}>------</option>
          <option key={0} value={"0"} selected={filters.isNegotiable === false}>No</option>
          <option key={1} value={"1"} selected={filters.isNegotiable === true}>Yes</option>
        </select><br />
        <label>Paid:</label><br />
        <select className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' onChange={x => setFilters({ ...filters, isPaid: x.target.value === "null" ? null : x.target.value === "1" ? true : false })}>
          <option key={null} value={"null"} selected={filters.isPaid === null}>------</option>
          <option key={0} value={"0"} selected={filters.isPaid === false}>No</option>
          <option key={1} value={"1"} selected={filters.isPaid === true}>Yes</option>
        </select><br />
        <label>Salary per hour min:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, salaryPerHourMin: Number(x.target.value) })} /><br />
        <label>Salary per hour max:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, salaryPerHourMax: Number(x.target.value) })} /><br />
        <label>Salary min:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, salaryMin: Number(x.target.value) })} /><br />
        <label>Salary max:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, salaryMax: Number(x.target.value) })} /><br />
        <label>Hours min:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, hoursMin: Number(x.target.value) })} /><br />
        <label>Hour max:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, hoursMax: Number(x.target.value) })} /><br />
        <label>Lon:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, lon: Number(x.target.value) })} /><br />
        <label>Lat:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, lat: Number(x.target.value) })} /><br />
        <label>Publish start from:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='date' onChange={x => setFilters({ ...filters, publicationStartFrom: new Date(x.target.value) })} /><br />
        <label>Publish start to:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='date' onChange={x => setFilters({ ...filters, publicationStartTo: new Date(x.target.value) })} /><br />
        <label>Publish end from:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='date' onChange={x => setFilters({ ...filters, publicationEndFrom: new Date(x.target.value) })} /><br />
        <label>Publish end to:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='date' onChange={x => setFilters({ ...filters, publicationEndTo: new Date(x.target.value) })} /><br />
        <label>Employment length from:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, employmentLengthFrom: Number(x.target.value) })} /><br />
        <label>Employment length to:</label><br />
        <input className='w-full h-[25px] rounded-md p-[3px] pt-[3px]' type='number' onChange={x => setFilters({ ...filters, employmentLengthTo: Number(x.target.value) })} /><br />
        <label>Contract conditions:</label><br />
        <ul>
          {contractParametersDictinary && contractParametersDictinary.map(x => (
            <li key={x.contractParameterId}>
              <input type='checkbox' value={x.contractParameterId} onChange={x => setFilters(
                {
                  ...filters, contractParameterIds: x.target.checked ?
                    [...filters.contractParameterIds, Number(x.target.value)] : filters.contractParameterIds.filter(y => y !== Number(x.target.value))
                })} />
              <label className='pl-[3px]'>{x.name}</label><br />
            </li>
          ))}
        </ul>
        <label>Skills:</label><br />
        <ul>
          {skillDictinary && skillDictinary.map(x => (
            <li key={x.skillId}>
              <input type='checkbox' value={x.skillId} onChange={x => setFilters(
                {
                  ...filters, skillsIds: x.target.checked ?
                    [...filters.skillsIds, Number(x.target.value)] : filters.skillsIds.filter(y => y !== Number(x.target.value))
                })} />
              <label className='pl-[3px]'>{x.name}</label><br />
            </li>
          ))}
        </ul>
      </div>
      <div className='flex-col ml-auto mr-auto text-center'>
        <div className=''>
          <h1 className="text-3xl font-bold mb-6 text-gray-800 dark:text-gray-100 text-center">Offers </h1>
          <ul>
            {apiData && apiData.totalCount > 0 ? (
              apiData.items.map(x => (
                <li key={x.offer.offerId} className='border 1px rounded-md p-4 text-left m-2 w-fit'>
                  <Link href={`/companies/${x.company.companyId}/${x.branch.branchId}/offer/${x.offer.offerId}`}>
                    <p className='font-bold flex items-center justify-between'>
                      <span className='pr-[5px]'>{x.offerTemplate.name}</span>{x.contractConditions && x.contractConditions.length > 0 && <span className='pl-[5px]'>{x.contractConditions[0].salaryMin}-{x.contractConditions[0].salaryMax}{x.contractConditions[0].currency.name}/{x.contractConditions[0].salaryTerm.name}</span>}
                    </p>
                    <p>
                      {x.company.name}
                    </p>
                    <p>
                      {x.branch.address.countryName}, {x.branch.address.stateName}, {x.branch.address.cityName}, {x.branch.address.streetName}
                    </p>
                  </Link>
                </li>
              ))) : (<>No offers</>)
            }
          </ul>
        </div><br />
        <div className='place-self-bottom'>
          {apiData && Array.from({ length: Math.ceil(apiData.totalCount / filters.itemsPerPage) }, (_, i) => (
            <button
              key={i + 1}
              onClick={() => setFilters({ ...filters, page: i + 1 })}
              disabled={filters.page === i + 1}
            >
              {i + 1}
            </button>
          ))}
        </div>
      </div>
    </div>
  )
}

export default Offers