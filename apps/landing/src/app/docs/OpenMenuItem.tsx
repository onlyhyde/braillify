'use client'
import { Box, css, Flex, Image, Text, VStack } from '@devup-ui/react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { Fragment, useReducer } from 'react'

import { MenuItemProps } from './MenuItem'

const URL_PREFIX = ''

export function OpenMenuItem({
  children,
  subMenu,
}: Omit<MenuItemProps, 'subMenu' | 'to'> &
  Required<Pick<MenuItemProps, 'subMenu'>>) {
  const path = usePathname()
  const selected = subMenu.some((item) =>
    item.to
      ? path.startsWith(URL_PREFIX + item.to) || path.startsWith(item.to)
      : false,
  )
  const [open, handleOpen] = useReducer((state) => !state, selected)
  return (
    <>
      <Flex
        _hover={{
          bg: '$menuHover',
        }}
        alignItems="center"
        bg={selected ? '$menuActive' : undefined}
        borderRadius="6px"
        cursor="pointer"
        gap="10px"
        onClick={handleOpen}
        p="6px 10px"
      >
        {selected && <Box bg="$primary" borderRadius="100%" boxSize="8px" />}
        <Text
          color={selected ? '$title' : '$text'}
          flex="1"
          opacity={selected ? '1' : '0.8'}
          typography={selected ? 'buttonLg' : 'button'}
        >
          {children}
        </Text>
        {subMenu && (
          <Image
            alt="Menu arrow icon"
            boxSize="16px"
            src="/images/docs/menu-arrow.svg"
            transform={open ? 'rotate(0deg)' : 'rotate(-90deg)'}
            transition="transform 0.2s"
          />
        )}
      </Flex>
      {open && (
        <Flex gap="8px">
          <Box borderRight="1px solid $border" w="10px" />
          <VStack flex="1" gap="4px">
            {subMenu.map(({ children, to }, idx) => {
              const selected = to
                ? path.startsWith(to) || path.startsWith(URL_PREFIX + to)
                : false
              const inner = (
                <Flex
                  _hover={{
                    bg: '$menuHover',
                  }}
                  alignItems="center"
                  bg={selected ? '$menuActive' : undefined}
                  borderRadius="6px"
                  gap="10px"
                  p="10px"
                >
                  {selected && (
                    <Box bg="$primary" borderRadius="100%" boxSize="8px" />
                  )}
                  <Text
                    color={selected ? '$title' : '$text'}
                    flex="1"
                    opacity={selected ? '1' : '0.8'}
                    typography={selected ? 'docsMenuBold' : 'docsMenu'}
                  >
                    {children}
                  </Text>
                </Flex>
              )

              return to ? (
                <Link
                  key={idx}
                  aria-label={`${children} link`}
                  className={css({
                    textDecoration: 'none',
                  })}
                  href={URL_PREFIX + to}
                >
                  {inner}
                </Link>
              ) : (
                <Fragment key={idx}>{inner}</Fragment>
              )
            })}
          </VStack>
        </Flex>
      )}
    </>
  )
}
